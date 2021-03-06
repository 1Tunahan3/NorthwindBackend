using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Log4Net.Loggers;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;

namespace Business.Concrete
{
  public  class ProductManager:IProductService
  {
      private IProductDal _productDal;

      public ProductManager(IProductDal productDal)
      {
          _productDal = productDal;
      }

      public IDataResult<Product> GetById(int productId)
      {
          return new SuccessDataResult<Product>(_productDal.Get(p => p.ProductId == productId));
      }

        public IDataResult<List<Product>> GetList()
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetList().ToList()); 
        }

        [SecuredOperation("Product.List,Admin")]
        [CacheAspect(duration:10)]
        [LogAspect(typeof(FileLogger))]
        public IDataResult<List<Product>>  GetListByCategory(int categoryId)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetList(p => p.CategoryId == categoryId).ToList()); 
        }


        [ValidationAspect(typeof(ProductValidator))]
        [CacheRemoveAspect("IProductService.Get")]
        public IResult Add(Product product)
        {
            ValidationTool.Validate(new ProductValidator(),product);
          
           _productDal.Add(product);
           return new SuccessResult(Messages.ProductAdded);
        }

        public IResult Delete(Product product)
        {
          
          _productDal.Delete(product);
          return new SuccessResult(Messages.ProductDeleted);
        }

        public IResult Update(Product product)
        {
            
            _productDal.Update(product);
            return new SuccessResult(Messages.ProductUpdated);
        }

        [TransactionAspect]
        public IResult Trans(Product product)
        {
            _productDal.Update(product);
            //_productDal.Add(product);
            return new SuccessResult(Messages.ProductUpdated);
        }
  }
}
