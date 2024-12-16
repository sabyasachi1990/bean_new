using AppsWorld.CommonModule.Entities;
using AppsWorld.CommonModule.Models;
using AppsWorld.CommonModule.RepositoryPattern;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using Repository.Pattern.Infrastructure;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
//using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace AppsWorld.CommonModule.Service
{
    public class DocRepositoryService : Service<DocRepository>, IDocRepositoryService
    {
        private readonly ICommonModuleRepositoryAsync<DocRepository> _docRepositroy;
        public DocRepositoryService(ICommonModuleRepositoryAsync<DocRepository> docRepositroy) : base(docRepositroy)
        {
            this._docRepositroy = docRepositroy;
        }
        //    //public void InsertDoc(DocRepository doc)
        //    //{
        //    //    _docRepositroy.Insert(doc);
        //    //}
        //    //public DocRepository GetAllDocRepIdCompanyDetailIdType(Guid id, Guid companyDetailId/*, string type*/)
        //    //{
        //    //    return _docRepositroy.Query(a => a.Id == id && a.TypeId == companyDetailId /*&& a.Type == type*/).Select().FirstOrDefault();
        //    //}
        //    //public void UpdateDoc(DocRepository doc)
        //    //{
        //    //    _docRepositroy.Update(doc);
        //    //}
        //    //public void DeleteDoc(DocRepository doc)
        //    //{
        //    //    _docRepositroy.Delete(doc);
        //    //}

        //    #region Nagendra



        //public DocRepositoryModel FillAttachements(List<DocRepositoryModel> attachments, Guid id)
        //{
        //    DocRepositoryModel doc = new DocRepositoryModel();
        //    if (attachments != null)
        //    {
        //        foreach (var attachment in attachments)
        //        {
        //            UpdateDocRepository(attachment, id);

        //            var compayRegistrationId = Guid.NewGuid();
        //            //if (compayRegistrationId != null)
        //            //    UploadMongoDBFile(attachment, compayRegistrationId, attachment.CompanyId);
        //            //else
        //            //    UploadMongoDBFile(attachment, id, attachment.CompanyId);

        //        }
        //    }
        //    return doc;
        //}



        //    #region mongosavesavefolderdata

        //static IMongoClient _client = /*new MongoClient(ConfigurationManager.AppSettings["mongodbconectionstring"]);*/string.Empty;
        //static IMongoDatabase database = _client.GetDatabase(ConfigurationManager.AppSettings["mongodbdatabasename"]);
        //private readonly string FileCollectionName = ConfigurationManager.AppSettings["FileCollectionName"];
        //private readonly string FileStorePath = ConfigurationManager.AppSettings["FileStorePath"];
        //public void UploadMongoDBFile(DocRepositoryModel lstdocrepository, Guid referenceId, long companyId)
        //{

        //    string file = null;
        //    try
        //    {
        //        var path1 = Path.GetFileName(lstdocrepository.FilePath);

        //        if (lstdocrepository.RecordStatus == "Added")
        //        {
        //            var httpRequest = HttpContext.Current.Request;

        //            if (httpRequest.Files.Count > 0)
        //            {
        //                for (int i = 0; i < httpRequest.Files.Count; i++)
        //                {
        //                    var fileData = httpRequest.Files[i];

        //                    double filesize = fileData.ContentLength;
        //                    filesize = (filesize / 1048576.0);
        //                    if (filesize >= 50.0)
        //                    {
        //                        throw new Exception("File Size is excceedded 50 MB");
        //                    }
        //                    var extension = fileData.FileName.Split('.').Last().ToUpper();
        //                    //var bucket = new GridFSBucket(database, new GridFSBucketOptions
        //                    //{
        //                    //    BucketName = FileCollectionName, ///Collection Name
        //                    //    ChunkSizeBytes = 1024 * 1024/// 1 MB = 1000 kilobytes
        //                    //});

        //                    var options = new GridFSUploadOptions
        //                    {
        //                        Metadata = new BsonDocument
        //     {
        //     { "ContentType", fileData.ContentType },

        //     { "ReferenceId",referenceId.ToString() },
        //     {"CompanyId",companyId },
        //     {"IsSystem",true}


        //     },
        //                        BatchSize = 45845,
        //                        ContentType = fileData.ContentType
        //                    };
        //                }

        //            }
        //            else
        //            {
        //                var fileData = lstdocrepository;

        //                double filesize = Convert.ToDouble(fileData.FileSize);
        //                filesize = (filesize / 1048576.0);
        //                if (filesize >= 50.0)
        //                {
        //                    throw new Exception("File Size is excceedded 50 MB");
        //                }
        //                var extension = path1.Split('.').Last().ToUpper();
        //                //var bucket = new GridFSBucket(database, new GridFSBucketOptions
        //                //{
        //                //    BucketName = FileCollectionName, ///Collection Name
        //                //    ChunkSizeBytes = 1024 * 1024/// 1 MB = 1000 kilobytes

        //                //});

        //                var options = new GridFSUploadOptions
        //                {
        //                    Metadata = new BsonDocument
        //     {
        //     { "ContentType",extension},
        //     { "ReferenceId",referenceId.ToString() },
        //     {"CompanyId",companyId },
        //     {"FileSize",fileData.FileSize.ToString()},
        //     {"FilePath",path1 },
        //     {"IsSystem" ,true },
        //     {"Isfolder",false }
        //     },
        //                    BatchSize = 45845,
        //                    ContentType = extension
        //                };

        //                WebClient client = new WebClient();

        //                string redirectUrl = /*ConfigurationManager.AppSettings["FileStorePath"] ??*/ String.Empty;
        //                file = Path.Combine(redirectUrl + "/" + path1);
        //                client.DownloadFile(new Uri(lstdocrepository.FilePath), file);
        //                FileStream source = new FileStream(file, FileMode.Open);

        //                //try
        //                //{
        //                //    var id = bucket.UploadFromStream(path1, source, options);
        //                //}
        //                //catch (Exception ex)
        //                //{

        //                //}

        //                source.Close();
        //                File.Delete(file);
        //            }
        //        }

        //        if (lstdocrepository.RecordStatus == "Deleted")
        //        {
        //            var bucket = new GridFSBucket(database, new GridFSBucketOptions
        //            {
        //                BucketName = FileCollectionName,
        //                ChunkSizeBytes = 1000/// 1 MB = 1000 kilobytes
        //            });

        //            var filter = Builders<GridFSFileInfo>.Filter.Where(a => a.Metadata["ReferenceId"] == referenceId.ToString() && a.Metadata["CompanyId"] == companyId && a.Filename == path1);
        //            var lstfilesinfo = bucket.Find(filter).FirstOrDefault();


        //            if (lstfilesinfo != null)
        //                bucket.Delete(lstfilesinfo.Id);

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}


        //    //#endregion

        //    //public DocRepositoryModel UpdateDocRepository(DocRepositoryModel docRepModel, Guid id)
        //    //{
        //    //    try
        //    //    {
        //    //        if (docRepModel != null)
        //    //        {

        //    //            if (docRepModel.RecordStatus == "Added")
        //    //            {
        //    //                DocRepository docRepository = new DocRepository();
        //    //                FillRepositorymodelToEntity(docRepository, docRepModel);
        //    //                docRepository.Id = Guid.NewGuid();
        //    //                docRepository.TypeId = id;
        //    //                docRepository.ObjectState = ObjectState.Added;
        //    //                InsertDoc(docRepository);

        //    //            }
        //    //            else if (docRepModel.RecordStatus == "Modified")
        //    //            {
        //    //                var doc = GetAllDocRepIdCompanyDetailIdType(docRepModel.Id, docRepModel.Id/*, docRepModel.Type*/);
        //    //                if (doc != null)
        //    //                {

        //    //                    FillRepositorymodelToEntity(doc, docRepModel);

        //    //                    doc.ObjectState = ObjectState.Modified;
        //    //                    UpdateDoc(doc);
        //    //                }
        //    //            }
        //    //            else if (docRepModel.RecordStatus == "Deleted")
        //    //            {
        //    //                if (docRepModel.Id != null)
        //    //                    DeleteDOCRepository(docRepModel.Id, docRepModel.Id, docRepModel.Type);//changes

        //    //            }
        //    //            try
        //    //            {

        //    //            }
        //    //            catch (DbEntityValidationException ex)
        //    //            {
        //    //                foreach (var eve in ex.EntityValidationErrors)
        //    //                {
        //    //                    Console.WriteLine(
        //    //                       "Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
        //    //                       eve.Entry.Entity.GetType().Name, eve.Entry.State);
        //    //                    foreach (var ve in eve.ValidationErrors)
        //    //                    {
        //    //                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
        //    //                           ve.PropertyName, ve.ErrorMessage);
        //    //                    }
        //    //                }
        //    //                throw ex;
        //    //            }
        //    //        }
        //    //    }
        //    //    catch (Exception ex)
        //    //    {
        //    //        throw ex;
        //    //    }


        //    //    return docRepModel;
        //    //}
        //    //private void FillRepositorymodelToEntity(DocRepository docRepository, DocRepositoryModel docRepModel)
        //    //{

        //    //    docRepository.CompanyId = docRepModel.CompanyId;
        //    //    docRepository.Type = docRepModel.Type;
        //    //    docRepository.CreatedDate = docRepModel.CreatedDate;
        //    //    docRepository.Description = docRepModel.Description;
        //    //    docRepository.FilePath = docRepModel.FilePath;
        //    //    docRepository.UserCreated = docRepModel.UserCreated;
        //    //    docRepository.FileExt = docRepModel.FileExt;
        //    //    docRepository.FileSize = docRepModel.FileSize;
        //    //    docRepository.ModifiedBy = docRepModel.ModifiedBy;
        //    //    docRepository.ModifiedDate = docRepModel.ModifiedDate;
        //    //    docRepository.ModuleName = "BR Cursor";
        //    //    docRepository.DisplayFileName = docRepModel.DisplayFileName;
        //    //    docRepository.NameofApprovalAuthority = docRepModel.NameofApprovalAuthority;
        //    //    docRepository.RecOrder = docRepModel.RecOrder;
        //    //    docRepository.Status = Framework.RecordStatusEnum.Active;
        //    //    docRepository.TypeIntId = docRepModel.TypeIntId;
        //    //}

        //    //public void DeleteDOCRepository(Guid id, Guid companyDetailId, string type)
        //    //{
        //    //    try
        //    //    {
        //    //        var existimgDoc = GetAllDocRepIdCompanyDetailIdType(id, companyDetailId/*, type*/);
        //    //        if (existimgDoc != null)
        //    //        {
        //    //            DeleteDoc(existimgDoc);
        //    //        }
        //    //    }
        //    //    catch (Exception ex)
        //    //    {
        //    //        throw ex;
        //    //    }

        //    //}
        //    #endregion

        //    //#region Get Files
        //    //public List<DocRepositoryModel> GetAllDocRepByIdType(Guid id, string type)
        //    //{
        //    //    {
        //    //        //var docrep = _docRepositroy.Queryable().Where(a => a.TypeId == id && a.Type == type).ToList(); 
        //    //        //List<DocRepositoryModel> DocrepModel = new List<DocRepositoryModel>();
        //    //        return (from doc in _docRepositroy.Queryable().Where(a => a.TypeId == id && a.Type == type)
        //    //                select new DocRepositoryModel()
        //    //                {
        //    //                    TypeId = doc.TypeId,
        //    //                    Id = doc.Id,
        //    //                    CompanyId = doc.CompanyId,
        //    //                    DisplayFileName = doc.DisplayFileName,
        //    //                    Description = doc.Description,
        //    //                    FilePath = doc.FilePath,
        //    //                    FileExt = doc.FileExt,
        //    //                    FileSize = doc.FileSize,
        //    //                    MongoFilesId = doc.MongoFilesId,
        //    //                    ModuleName = doc.ModuleName,
        //    //                    NameofApprovalAuthority = doc.NameofApprovalAuthority,
        //    //                    RecOrder = doc.RecOrder,
        //    //                    Status = doc.Status,
        //    //                    Type = doc.Type,
        //    //                    //Fe = doc.TypeId,
        //    //                    TypeIntId = doc.TypeIntId,
        //    //                    TypeKey = doc.TypeKey,
        //    //                    UserCreated = doc.UserCreated,
        //    //                    Version = doc.Version,
        //    //                    CreatedDate = DateTime.UtcNow,
        //    //                    ModifiedBy = doc.ModifiedBy,
        //    //                    ModifiedDate = doc.ModifiedDate
        //    //                }).ToList().OrderByDescending(a => a.CreatedDate).ToList();
        //    //        //return lst.ToList();
        //    //    }
        //    //}
        //    //#endregion Get Files
    }
}

