using System;

using ObjCRuntime;
using Foundation;
using UIKit;
using CoreNFC;

namespace NativeLibrary
{
    // @interface NFCError : NSObject
    [BaseType(typeof(NSObject))]
    interface NFCError
    {
        // @property (nonatomic) NFCErrorType errorType;
        [Export("errorType", ArgumentSemantic.Assign)]
        NFCErrorType ErrorType { get; set; }

        // @property (copy, nonatomic) NSString * desc;
        [Export("desc")]
        string Desc { get; set; }

        // +(instancetype)errorWithType:(NFCErrorType)type Desc:(NSString *)desc;
        [Static]
        [Export("errorWithType:Desc:")]
        NFCError ErrorWithType(NFCErrorType type, string desc);
    }

    // @interface NFCLib : NSObject
    [BaseType(typeof(NSObject))]
    interface NFCLib
    {
        // +(instancetype)sharedInstance;
        [Static]
        [Export("sharedInstance")]
        NFCLib SharedInstance();

        // +(_Bool)isSupportedTag:(id<NFCTag>)tag;
        [Static]
        [Export("isSupportedTag:")]
        bool IsSupportedTag(INFCTag tag);

        // -(void)getEslIdAction:(id<NFCTag>)tag FinishBlock:(void (^)(int *))finishBlock;
        [Export("getEslIdAction:FinishBlock:")]
        unsafe void GetEslIdAction(INFCTag tag, Action<int> finishBlock);
    }

    // @interface NFCResponse : NSObject
    [BaseType(typeof(NSObject))]
    interface NFCResponse
    {
        // @property (nonatomic, strong) NSDictionary * data;
        [Export("data", ArgumentSemantic.Strong)]
        NSDictionary Data { get; set; }

        // @property (nonatomic, strong) NFCError * error;
        [Export("error", ArgumentSemantic.Strong)]
        NFCError Error { get; set; }

        // +(instancetype)responseWithData:(NSDictionary *)data Error:(NFCError *)error;
        [Static]
        [Export("responseWithData:Error:")]
        NFCResponse ResponseWithData(NSDictionary data, NFCError error);
    }

}


