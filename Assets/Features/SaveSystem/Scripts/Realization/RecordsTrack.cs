using Assets.Features.SaveSystem.Scripts;
using Assets.Features.SaveSystem.Scripts.Interfaces;
using System;
using System.Collections.Generic;

public class RecordsTrack : IRecordsTrack
{
    private IList<RecordData> _scoreRecordData = new List<RecordData>();
    public void Dispose()
    {
        _scoreRecordData.Clear();
        _scoreRecordData = null;
    }
    public void AddRecord(RecordData data)
    {
        _scoreRecordData.Add(data);
    }

    public bool CheckScore(int score)
    {
        foreach (var record in _scoreRecordData)
        {
            if(score > record.Value)
            {
                return true;
            }
        }

        return false;
    }
   
    public IEnumerable<RecordData> GetAllRecods()
    {
        return _scoreRecordData;
    }
}
