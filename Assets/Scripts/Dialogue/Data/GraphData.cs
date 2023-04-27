using System;

namespace Dialogue.Data
{
    [Serializable]
    public class GraphData
    {
        public int graphAmount;
        public int charactersAmount;
        public byte[] maxGraphAmount;
        public byte[] maxCharactersAmount;

        public GraphData(byte[] maxG, byte[] maxC)
        {
            maxGraphAmount = maxG;
            maxCharactersAmount = maxC;
        }

        public GraphData(int graphAmount, int charactersAmount)
        {
            this.graphAmount = graphAmount;
            this.charactersAmount = charactersAmount;
        }
    }
}