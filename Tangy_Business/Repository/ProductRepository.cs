using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tangy_Business.Repository.IRepository;
using Tangy_DataAccess;
using Tangy_DataAccess.Data;
using Tangy_Models;

namespace Tangy_Business.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public ProductRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<ProductDTO> Create(ProductDTO productDTO)
        {
            var obj = _mapper.Map<ProductDTO, Product>(productDTO);
            var addedObj = await _db.AddAsync(obj);
            await _db.SaveChangesAsync();
            return _mapper.Map<Product, ProductDTO>(addedObj.Entity);
        }

        public async Task<int> Delete(int id)
        {
            var obj = await _db.Products.FirstOrDefaultAsync(u => u.Id == id);
            if (obj != null)
            {
                _db.Products.Remove(obj);
                return await _db.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<ProductDTO> Get(int id)
        {
            var obj = await _db.Products.Include(u => u.Category).FirstOrDefaultAsync(u => u.Id == id);
            if (obj != null)
            {
                return _mapper.Map<Product, ProductDTO>(obj);
            }
            return new ProductDTO();
        }

        public async Task<IEnumerable<ProductDTO>> GetAll()
        {
            return _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(_db.Products.Include(u => u.Category));
        }

        public async Task<ProductDTO> Update(ProductDTO productDTO)
        {
            var obj = await _db.Products.FirstOrDefaultAsync(u => u.Id == productDTO.Id);
            if (obj != null)
            {
                obj = _mapper.Map<ProductDTO, Product>(productDTO);
                _db.Products.Update(obj);
                await _db.SaveChangesAsync();
                return _mapper.Map<Product, ProductDTO>(obj);
            }
            else return productDTO;
        }
    }
}
