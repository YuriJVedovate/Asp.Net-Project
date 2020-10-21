using Microsoft.EntityFrameworkCore;
using MimicAPI.Database;
using MimicAPI.Helpers;
using MimicAPI.V1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.V1.Repositories.Contracts
{
    public class WordRepository : IWordRepository
    {
        private readonly MimicContext _base;
        public WordRepository(MimicContext banco) => _base = banco;


        public PaginationList<Word> GetAll(WordQueryUrl query)
        {
            var lista = new PaginationList<Word>();
            var item = _base.Words.AsNoTracking().AsQueryable();
            if (query.Date.HasValue)
            {
                item = item.Where(a => a.Creation >= query.Date.Value || a.Update > query.Date.Value);
            }

            if (query.NumPage.HasValue)
            {

                var TotalNumberRecords = item.Count();
                item = item.Skip((query.NumPage.Value - 1) * query.NumRecordsPage.Value).Take(query.NumRecordsPage.Value);

                var pagination = new Pagination();
                pagination.NumberPage = query.NumPage.Value;
                pagination.NumberRecords = query.NumRecordsPage.Value;
                pagination.TotalPages = (int)Math.Ceiling((double)TotalNumberRecords / query.NumRecordsPage.Value);
                pagination.TotalRecords = TotalNumberRecords;

                lista.Pagination = pagination;
            }
            lista.Results.AddRange(item.ToList());
            
            return lista;
        }

        public Word GetOne(int id)
        {
           return _base.Words.AsNoTracking().FirstOrDefault(a => a.Id == id);
        }

        public void Register(Word Words)
        {
            _base.Words.Add(Words);
            _base.SaveChanges();
        }

        public void Update(Word Words)
        {
            
            _base.Words.Update(Words);
            _base.SaveChanges();
        }

        public void Delete(int id)
        {
            var word = GetOne(id);
            word.Status = false;
            _base.Words.Update(word);
            _base.SaveChanges();
        }
    }
}
