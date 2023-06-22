using System;
using System.Threading.Tasks;

namespace NFCDemo.Services
{
	public interface IESLService
	{
		Task StartScan();
		Task StopScan();
		void FakeESL();
		event EventHandler<int> OnESLIdDetected;
	}
}

