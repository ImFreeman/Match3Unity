using System;
using System.Collections.Generic;

namespace Assets.Features.SaveSystem.Scripts.Interfaces
{
    public interface IRecordsTrack : IDisposable
    {
        public bool CheckScore(int score);
        public void AddRecord(RecordData data);        
        public IEnumerable<RecordData> GetAllRecods();        
    }
}
