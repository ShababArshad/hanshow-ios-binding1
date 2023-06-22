using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreFoundation;
using CoreNFC;
using Foundation;
using NFCDemo.Services;
using ObjCRuntime;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(NFCDemo.iOS.Services.ESLService))]
namespace NFCDemo.iOS.Services
{

    
    public class ESLService : IESLService,IDisposable
    {
        NFCImplementation nfcService;
 

        public event EventHandler<int> OnESLIdDetected;

        public ESLService()
        {
            nfcService = new NFCImplementation();
          
            nfcService.OnGetEslID += NfcService_OnGetEslID;
        }

        private void NfcService_OnGetEslID(object sender, int e)
        {
            OnESLIdDetected?.Invoke(sender, e);
        }
         
       void IESLService.FakeESL()
        {
            nfcService.FakeESL();
        }
        public void Dispose()
        {
            nfcService.StopListening();
            nfcService.Dispose();

        }

        public Task StartScan()
        {
            nfcService?.StartListening();
            return Task.CompletedTask;
        }

        public Task StopScan()
        {
            nfcService?.StopListening();
            return Task.CompletedTask;
        }
    }
    public class NFCImplementation : NFCTagReaderSessionDelegate
    {

        public const string SessionTimeoutMessage = "session timeout";

        public event EventHandler OnTagConnected;
        public event EventHandler OnTagDisconnected;
        public event EventHandler<int> OnGetEslID;

        bool _isWriting;
        bool _isFormatting;
        bool _customInvalidation = false;

        INFCTag _tag;

        NFCTagReaderSession NfcSession { get; set; }

        /// <summary>
        /// Checks if NFC Feature is available
        /// </summary>
        public bool IsAvailable => NFCReaderSession.ReadingAvailable;

        /// <summary>
        /// Checks if NFC Feature is enabled
        /// </summary>
        public bool IsEnabled => IsAvailable;

        /// <summary>
        /// Checks if writing mode is supported
        /// </summary>
        public bool IsWritingTagSupported => true;

        /// <summary>
		/// Starts tags detection
		/// </summary>
		public void StartListening()
        {
            _customInvalidation = false;
            _isWriting = false;
            _isFormatting = false;

            NfcSession = new NFCTagReaderSession(NFCPollingOption.Iso14443 | NFCPollingOption.Iso15693, this, DispatchQueue.CurrentQueue)
            {
                AlertMessage = "Alert"
            };
            NfcSession?.BeginSession();
            
        }

        public void FakeESL()
        {
            NativeLibrary.NFCLib nFCLib = new NativeLibrary.NFCLib();
            var session = new NFCTagReaderSession(NFCPollingOption.Iso14443 | NFCPollingOption.Iso15693, this, DispatchQueue.CurrentQueue)
            {
                AlertMessage = "Alert"
            };
            session?.BeginSession();
            INFCTag tag = new NFCTag(session);
            nFCLib.GetEslIdAction(tag, (t) =>
            {
                OnGetEslID?.Invoke(this,t);
            });
        }
        /// <summary>
        /// Stops tags detection
        /// </summary>
        public void StopListening()
        {
            NfcSession?.InvalidateSession();
        }

        /// <summary>
		/// Event raised when NFC tags are detected
		/// </summary>
		/// <param name="session">iOS <see cref="NFCTagReaderSession"/></param>
		/// <param name="tags">Array of iOS <see cref="INFCTag"/></param>
		public override void DidDetectTags(NFCTagReaderSession session, INFCTag[] tags)
        {
            _customInvalidation = false;
            _tag = tags.First();

            var connectionError = string.Empty;
            session.ConnectTo(_tag, (error) =>
            {
                if (error != null)
                {
                    connectionError = error.LocalizedDescription;
                    Invalidate(session, connectionError);
                    return;
                }
                NativeLibrary.NFCLib nFCLib = new NativeLibrary.NFCLib();
               
             
                var ndefTag = NfcNdefTagExtensions.GetNdefTag(_tag);

                if (ndefTag == null)
                {
                    Invalidate(session, "Non-Complaint Tag");
                    return;
                }
             

                ndefTag.QueryNdefStatus((status, capacity, _error) =>
                {
                    if (_error != null)
                    {
                        Invalidate(session, _error.LocalizedDescription);
                        return;
                    }

                    var isNdefSupported = status != NFCNdefStatus.NotSupported;

                    var identifier = NfcNdefTagExtensions.GetTagIdentifier(ndefTag);
                   
                    if (!isNdefSupported)
                    {
                        session.AlertMessage = "Not supported";
                         
                        Invalidate(session);
                        return;
                    }

                    if (_isWriting)
                    {
                        // Write mode
                        
                    }
                    else
                    {
                        // Read mode
                        ndefTag.ReadNdef((message, __error) =>
                        {
                            // iOS Error: NFCReaderError.NdefReaderSessionErrorZeroLengthMessage (NDEF tag does not contain any NDEF message)
                            // NFCReaderError.NdefReaderSessionErrorZeroLengthMessage constant should be equals to 403 instead of 304
                            // see https://developer.apple.com/documentation/corenfc/nfcreadererror/code/ndefreadersessionerrorzerolengthmessage
                            if (__error != null && __error.Code != 403)
                            {
                                Invalidate(session, "Error on Read");
                                return;
                            }

                            session.AlertMessage = "Success";

                            nFCLib.GetEslIdAction(_tag, (t) =>
                            {
                                OnGetEslID?.Invoke(this, t);
                            });
                            Invalidate(session);
                        });
                    }
                });
            });
        }
        

        /// <summary>
        /// Event raised when an error happened during detection
        /// </summary>
        /// <param name="session">iOS <see cref="NFCTagReaderSession"/></param>
        /// <param name="error">iOS <see cref="NSError"/></param>
        public override void DidInvalidate(NFCTagReaderSession session, NSError error)
        {
         

            //var readerError = (NFCReaderError)(long)error.Code;
            //if (readerError != NFCReaderError.ReaderSessionInvalidationErrorFirstNDEFTagRead && readerError != NFCReaderError.ReaderSessionInvalidationErrorUserCanceled)
            //{
            //    var alertController = UIAlertController.Create(Configuration.Messages.NFCSessionInvalidated, error.LocalizedDescription.ToLower().Equals(SessionTimeoutMessage) ? Configuration.Messages.NFCSessionTimeout : error.LocalizedDescription, UIAlertControllerStyle.Alert);
            //    alertController.AddAction(UIAlertAction.Create(Configuration.Messages.NFCSessionInvalidatedButton, UIAlertActionStyle.Default, null));
            //    OniOSReadingSessionCancelled?.Invoke(null, EventArgs.Empty);
            //    DispatchQueue.MainQueue.DispatchAsync(() =>
            //    {
            //        GetCurrentController().PresentViewController(alertController, true, null);
            //    });
            //}
            //else if (readerError == NFCReaderError.ReaderSessionInvalidationErrorUserCanceled && !_customInvalidation)
            //    OniOSReadingSessionCancelled?.Invoke(null, EventArgs.Empty);
        }

        /// <summary>
        /// Invalidate the session
        /// </summary>
        /// <param name="session"><see cref="NFCTagReaderSession"/></param>
        /// <param name="message">Message to show</param>
        void Invalidate(NFCTagReaderSession session, string message = null)
        {
            _customInvalidation = true;
            if (string.IsNullOrWhiteSpace(message))
                session.InvalidateSession();
            else
                session.InvalidateSession(message);
        }

    }

    /// <summary>
	/// NFC Tag Extensions Class
	/// </summary>
	internal static class NfcNdefTagExtensions
    {
        /// <summary>
        /// Get Ndef tag
        /// </summary>
        /// <param name="tag"><see cref="INFCTag"/></param>
        /// <returns><see cref="INFCNdefTag"/></returns>
        internal static INFCNdefTag GetNdefTag(INFCTag tag)
        {
            if (tag == null || !tag.Available)
                return null;

            INFCNdefTag ndef;

#if NET6_0_OR_GREATER
			if (tag.Type == CoreNFC.NFCTagType.MiFare)
				ndef = tag.AsNFCMiFareTag;
			else if (tag.Type == CoreNFC.NFCTagType.Iso7816Compatible)
				ndef = tag.AsNFCIso7816Tag;
			else if (tag.Type == CoreNFC.NFCTagType.Iso15693)
				ndef = tag.AsNFCIso15693Tag;
			else if (tag.Type == CoreNFC.NFCTagType.FeliCa)
				ndef = tag.AsNFCFeliCaTag;
			else
				ndef = null;
#else
            if (tag.GetNFCMiFareTag() != null)
                ndef = tag.GetNFCMiFareTag();
            else if (tag.GetNFCIso7816Tag() != null)
                ndef = tag.GetNFCIso7816Tag();
            else if (tag.GetNFCIso15693Tag() != null)
                ndef = tag.GetNFCIso15693Tag();
            else if (tag.GetNFCFeliCaTag() != null)
                ndef = tag.GetNFCFeliCaTag();
            else
                ndef = null;
#endif

            return ndef;
        }

        /// <summary>
        /// Returns NFC Tag identifier
        /// </summary>
        /// <param name="tag"><see cref="INFCNdefTag"/></param>
        /// <returns>Tag identifier</returns>
        internal static byte[] GetTagIdentifier(INFCNdefTag tag)
        {
            byte[] identifier = null;
            if (tag is INFCMiFareTag mifareTag)
            {
                identifier = mifareTag.Identifier.ToByteArray();
            }
            else if (tag is INFCFeliCaTag felicaTag)
            {
                identifier = felicaTag.CurrentIdm.ToByteArray();
            }
            else if (tag is INFCIso15693Tag iso15693Tag)
            {
                identifier = iso15693Tag.Identifier.ToByteArray().Reverse().ToArray();
            }
            else if (tag is INFCIso7816Tag iso7816Tag)
            {
                identifier = iso7816Tag.Identifier.ToByteArray();
            }
            return identifier;
        }

        /// <summary>
		/// Convert an iOS <see cref="NSData"/> into an array of bytes
		/// </summary>
		/// <param name="data">iOS <see cref="NSData"/></param>
		/// <returns>Array of bytes</returns>
		public static byte[] ToByteArray(this NSData data)
        {
            var bytes = new byte[data.Length];
            if (data.Length > 0) System.Runtime.InteropServices.Marshal.Copy(data.Bytes, bytes, 0, Convert.ToInt32(data.Length));
            return bytes;
        }
    }

    internal sealed class NFCTag : NSObject, INFCTag
    {
        NFCReaderSession _session;
        public NFCTag(NFCReaderSession session)
        {
            this._session = session;
        }

        public NFCTagType Type => NFCTagType.Iso15693;

        public NFCReaderSession Session => _session;

        public bool Available => _session != null;

      

        [return: Release]
        public NSObject Copy(NSZone zone)
        {
            return this;
        }

         

        public void EncodeTo(NSCoder encoder)
        {
            this.GetNFCIso15693Tag().EncodeTo(encoder);
        }
    }
}

