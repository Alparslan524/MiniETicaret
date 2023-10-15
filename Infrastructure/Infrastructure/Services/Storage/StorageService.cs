using Application.Abstractions.Storage;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Storage
{
    //Kullanılacak teknolojiler IStorage'den implemente alıcak(ILocalStorage,IAzureStogare vs)
    //IStorageService de IStorageden implemente alıcak. StorageService de IStorageServiceden implemente alıcak.
    //StorageService'de yani burda IStorage metotları kullanılacak. Aslında ServiceRegistrationda belirttiğimiz teknolojinin yani
    //misal LocalStorage metodları çalışacak. Çünkü ServiceRegistrationda şöyle dedik => IStorage isteyen olursa
    //program.cs deki generic yapıya verileni ver. 
    //Misal şu an program.cs de şu yazılı => builder.Services.AddStorage<LocalStorage>();
    //Bu yüzden StorageService kullanıldığı zaman _storage.DeleteAsync metodu aslında LocalStoragede yazdığımız DeleteAsync metoduna denk geliyor.
    //Biz Teknolojiyi değiştirmek istediğimizde ise sadece LocalStorage yerine AzureStorage yazıcak ve bitecek
    public class StorageService : IStorageService
    {
        readonly IStorage _storage;

        public StorageService(IStorage storage)
        {
            _storage = storage;
        }

        public string StorageName { get => _storage.GetType().Name; }//Storage tipini vericek

        public async Task DeleteAsync(string pathOrContainerName, string fileName)
        {
            await _storage.DeleteAsync(pathOrContainerName, fileName);
        }

        public List<string> GetFiles(string pathOrContainerName)
        {
            return _storage.GetFiles(pathOrContainerName);
        }

        public bool HasFile(string pathOrContainerName, string fileName)
        {
            return _storage.HasFile(pathOrContainerName, fileName);
        }

        public Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string pathOrContainerName, IFormFileCollection files)
        {
            return _storage.UploadAsync(pathOrContainerName, files);
        }
    }
}
