using System.Collections.Generic;

namespace MVVM_WPF
{
    interface IFileService
    {
        List<Phone> Open(string fileName);
        void Save(string fileName, List<Phone> phoneList);
    }
}
