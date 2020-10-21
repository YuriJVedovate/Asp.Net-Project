using MimicAPI.Helpers;
using MimicAPI.V1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.V1.Repositories.Contracts
{
    public interface IWordRepository
    {
        PaginationList<Word> GetAll(WordQueryUrl query);

        Word GetOne(int id);

        void Register(Word words);

        void Update(Word words);

        void Delete(int id);


    }
}
