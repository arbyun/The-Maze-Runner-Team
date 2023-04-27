using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Dialogue.Data
{
    public static class Graphfl
    {
        private static int Graphl1M()
        {
            if (File.Exists(GraphflC.GA))
            {
                var formatter = new BinaryFormatter();
                using var stream = new FileStream(GraphflC.GA, FileMode.Open);
                var data = formatter.Deserialize(stream) as GraphData;
                stream.Close();
                return ByteArrayToObject(data.maxGraphAmount);
            }

            return 1;
        }

        private static int Graphl2M()
        {
            if (File.Exists(GraphflC.GA))
            {
                var formatter = new BinaryFormatter();
                using var stream = new FileStream(GraphflC.GA, FileMode.Open);
                var data = formatter.Deserialize(stream) as GraphData;
                stream.Close();
                return ByteArrayToObject(data.maxCharactersAmount);
            }

            return 1;
        }

        public static bool Graphl1L()
        {
            var loadedData = LoadData();
            if (loadedData == null)
            {
                SaveData(1, 0);
                return true;
            }

            SaveData(loadedData.graphAmount + 1, loadedData.charactersAmount);
            return loadedData.graphAmount <= Graphl1M();
        }

        public static bool Graphl2L()
        {
            var loadedData = LoadData();
            if (loadedData == null)
            {
                SaveData(0, 1);
                return true;
            }

            SaveData(loadedData.graphAmount, loadedData.charactersAmount + 1);
            return loadedData.charactersAmount <= Graphl2M();
        }

        public static void SaveData(int graphAmount, int charactersAmount)
        {
            var formatter = new BinaryFormatter();
            using var stream = new FileStream(GraphflC.GB, FileMode.Create);
            var data = new GraphData(graphAmount, charactersAmount);
            formatter.Serialize(stream, data);
            stream.Close();
        }

        public static GraphData LoadData()
        {
            if (File.Exists(GraphflC.GB))
            {
                var formatter = new BinaryFormatter();
                using var stream = new FileStream(GraphflC.GB, FileMode.Open);
                var data = formatter.Deserialize(stream) as GraphData;
                stream.Close();
                return data;
            }

            return null;
        }

        private static byte[] ObjectToByteArray(int obj)
        {
            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        private static int ByteArrayToObject(byte[] arrBytes)
        {
            using (var memStream = new MemoryStream())
            {
                var binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                var obj = binForm.Deserialize(memStream);
                return (int)obj;
            }
        }

        public static void Stm()
        {
            if (File.Exists(GraphflC.GA)) return;

            var toSave = ObjectToByteArray(3);
            var toSave2 = ObjectToByteArray(5);
            var formatter = new BinaryFormatter();
            using (var stream = new FileStream(GraphflC.GA, FileMode.Create))
            {
                var data = new GraphData(toSave, toSave2);
                formatter.Serialize(stream, data);
                stream.Close();
            }
        }
    }
}