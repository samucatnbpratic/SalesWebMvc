using SalesWebMvc.Data;
using SalesWebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SalesWebMvc.Services
{
    public class SalesRecordService
    {
        //readonly previne que a dependencia nao sera alterada
        private readonly SalesWebMvcContext _context;

        public SalesRecordService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            //pega o SalesRecord que é do tipo dbSet e construir um objeto result do tipo IQueryAble
            //em cima deste objeto podemos acrescentar outros detalhes da consulta
            //lembrando: essa consulta não é executa pela simples declaração abaixo.
            var result = from obj in _context.SalesRecord select obj;

            if (minDate.HasValue)
            {
                result = result.Where(x => x.Date >= minDate.Value);
            }
            if (maxDate.HasValue)
            {
                result = result.Where(x => x.Date <= maxDate.Value);
            }
            
            //aqui é onde é executado a consulta
            return await result 
                .Include(x => x.Seller)  // fazemo o Join com Seller
                .Include(x => x.Seller.Department) //fazemo o Join com Department
                .OrderByDescending(x => x.Date)
                .ToListAsync();
        }
        public async Task<List<IGrouping<Department, SalesRecord>>> FindByDateGroupingAsync(DateTime? minDate, DateTime? maxDate)
        {
            //pega o SalesRecord que é do tipo dbSet e construir um objeto result do tipo IQueryAble
            //em cima deste objeto podemos acrescentar outros detalhes da consulta
            //lembrando: essa consulta não é executa pela simples declaração abaixo.
            var result = from obj in _context.SalesRecord select obj;

            if (minDate.HasValue)
            {
                result = result.Where(x => x.Date >= minDate.Value);
            }
            if (maxDate.HasValue)
            {
                result = result.Where(x => x.Date <= maxDate.Value);
            }

            //aqui é onde é executado a consulta
            return await result
                .Include(x => x.Seller)  // fazemo o Join com Seller
                .Include(x => x.Seller.Department) //fazemo o Join com Department
                .OrderByDescending(x => x.Date)
                .GroupBy(x => x.Seller.Department)
                .ToListAsync();
        }
    }
}
