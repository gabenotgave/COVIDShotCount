using COVIDVaccinationCount.Data.Models;
using MongoDB.Driver;
using System.Linq;

namespace COVIDVaccinationCount.Data
{
    class MongoCRUD
    {
        private IMongoDatabase db;
        public MongoCRUD(string database)
        {
            this.db = new MongoClient(Credentials.GetValue("MongoDB_CS")).GetDatabase(database);
        }

        public void InsertRecord<T>(string table, T record)
        {
            var collection = db.GetCollection<T>(table);
            collection.InsertOne(record);
        }

        public int GetLatestVaccinationCount()
        {
            var collection = db.GetCollection<VaccinationRecord>("Vaccinations");
            return collection.AsQueryable().OrderByDescending(x => x.DateTimeAdded).FirstOrDefault().FirstDosesAdministered;
        }
    }
}
