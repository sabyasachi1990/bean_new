//using Appsworld.CommonModule.RepositoryPattern.MongoRepository;
//using MongoDB.Bson;
//using MongoDB.Bson.Serialization.Attributes;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace AppsWorld.CommonModule.Entites.Models
//{
//    public class ReferenceFiles : IEntity
//    {

//        [BsonId]
//        [BsonRepresentation(BsonType.ObjectId)]
//        public string Id { get; set; }


//        public string FileId { get; set; }

//        public string ReferenceId { get; set; }

//        public string ContentType { get; set; }

//        public long CompanyId { get; set; }


//        public string TabName { get; set; }

//        public string FileSize { get; set; }

//        public string FilePath { get; set; }
//        public string UserCreated { get; set; }
//        public string ModifiedBy { get; set; }
//        public DateTime CreatedDate { get; set; }
//        public DateTime ModifiedDate { get; set; }

//        public string ModuleName { get; set; }
//        public bool Isfolder { get; set; }

//        public bool IsSuccess { get; set; }
//        public bool IsSystem { get; set; }

//        public string FileExt { get; set; }

//        public string FeatureId { get; set; }

//        public string FileName { get; set; }

//        public string Source { get; set; }
//    }
//}