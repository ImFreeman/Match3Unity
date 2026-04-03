using System;

namespace Assets.Features.SaveSystem.Scripts
{
    [Serializable]
    public class RecordData
    {       
        public DateTime Date { get; set; }         
        public int Value { get; set; }
    }
}
