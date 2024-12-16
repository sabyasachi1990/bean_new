

//using AppsWorld.CommonModule.Entities;
//using AppsWorld.CommonModule.RepositoryPattern.MongoRepository;
//using MongoDB.Bson;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace AppsWorld.CommonModule.Service
//{
//    public class ReferenceFilesService : MongoRepository<ReferenceFiles>, IReferenceFilesService
//    {
//        private readonly MongoRepository<ReferenceFiles> _referenceFilesService;
//        public ReferenceFilesService(MongoRepository<ReferenceFiles> referenceFilesService)
//        {
//            this._referenceFilesService = referenceFilesService;
//        }
//        public void DeleteDoc(string id)
//        {
//            _referenceFilesService.Delete(ObjectId.Parse(id));
//        }
//        public ReferenceFiles GetReferenceFiles(string mongoFilesId)
//        {
//            return _referenceFilesService.Where(a => a.FileId == mongoFilesId).FirstOrDefault();
//        }
//        public List<ReferenceFiles> GetReferenceFiles(string mongoFilesId, string id)
//        {
//            return _referenceFilesService.Where(a => a.FileId == ObjectId.Parse(mongoFilesId).ToString() && a.ReferenceId == id).ToList();
//        }
//    }
//}

