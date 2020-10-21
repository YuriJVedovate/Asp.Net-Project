using MimicAPI.Helpers;
using MimicAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.Repositories.Contracts
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
