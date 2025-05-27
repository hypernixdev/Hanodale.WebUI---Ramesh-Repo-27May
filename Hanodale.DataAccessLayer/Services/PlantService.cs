using Hanodale.DataAccessLayer.Interfaces;
using Hanodale.Domain.DTOs;
using Hanodale.Entity.Core;
using System;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.ServiceModel;
using System.Xml.Linq;

namespace Hanodale.DataAccessLayer.Services
{
    public class PlantService : BaseService, IPlantService
    {
        public PlantDetails GetPlantBySearch(DatatableFilters entityFilter)
        {
            PlantDetails _result = new PlantDetails();
            _result.recordDetails = new RecordDetails();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    if (entityFilter == null)
                        entityFilter = new DatatableFilters();

                    var query = model.Plant.AsNoTracking().Where(p => true);

                    _result.recordDetails.totalRecords = query.Count();
                    _result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;

                    if (!string.IsNullOrEmpty(entityFilter.search))
                    {
                        query = query.Where(p => (
                                p.company.Contains(entityFilter.search)
                                || p.plant1.Contains(entityFilter.search)
                                || p.name.Contains(entityFilter.search)
                                || p.address1.Contains(entityFilter.search)
                                || p.address2.Contains(entityFilter.search)
                                || p.city.Contains(entityFilter.search)
                            ));
                    }

                    var result = query.OrderByDescending(p => p.id)
                        .Select(p => new Plants
                        {
                            id = p.id,
                            company = p.company,
                            plant = p.plant1,
                            name = p.name,
                            address1 = p.address1,
                            address2 = p.address2,
                            address3 = p.address3,
                            city = p.city,
                            state = p.state,
                            zip = p.zip
                        });

                    _result.recordDetails.totalDisplayRecords = result.Count();
                    _result.lstPlant = result.Skip(entityFilter.startIndex).Take(entityFilter.pageSize).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _result;
        }

        public Plants CreatePlant(Plants entityEn)
        {
            var _PlantEn = new Entity.Core.Plant();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    _PlantEn.company = entityEn.company;
                    _PlantEn.plant1 = entityEn.plant;
                    _PlantEn.name = entityEn.name;
                    _PlantEn.address1 = entityEn.address1;
                    _PlantEn.address2 = entityEn.address2;
                    _PlantEn.address3 = entityEn.address3;
                    _PlantEn.city = entityEn.city;
                    _PlantEn.state = entityEn.state;
                    _PlantEn.zip = entityEn.zip;

                    model.Plant.Add(_PlantEn);
                    model.SaveChanges();

                    entityEn.id = _PlantEn.id;
                    entityEn.isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return entityEn;
        }

        public Plants UpdatePlant(Plants entityEn)
        {
            var _PlantEn = new Entity.Core.Plant();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    _PlantEn = model.Plant.SingleOrDefault(p => p.id == entityEn.id);
                    if (_PlantEn != null)
                    {
                        _PlantEn.company = entityEn.company;
                        _PlantEn.plant1 = entityEn.plant;
                        _PlantEn.name = entityEn.name;
                        _PlantEn.address1 = entityEn.address1;
                        _PlantEn.address2 = entityEn.address2;
                        _PlantEn.address3 = entityEn.address3;
                        _PlantEn.city = entityEn.city;
                        _PlantEn.state = entityEn.state;
                        _PlantEn.zip = entityEn.zip;

                        model.SaveChanges();

                        entityEn.isSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return entityEn;
        }

        public bool DeletePlant(int id)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var _PlantEn = model.Plant.SingleOrDefault(p => p.id == id);
                    if (_PlantEn != null)
                    {
                        model.Plant.Remove(_PlantEn);
                        model.SaveChanges();
                        return true;
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        public Plants GetPlantById(int id)
        {
            Plants _PlantEn = new Plants();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var entity = model.Plant.SingleOrDefault(p => p.id == id);
                    if (entity != null)
                    {
                        _PlantEn = new Plants
                        {
                            id = entity.id,
                            company = entity.company,
                            plant = entity.plant1,
                            name = entity.name,
                            address1 = entity.address1,
                            address2 = entity.address2,
                            address3 = entity.address3,
                            city = entity.city,
                            state = entity.state,
                            zip = entity.zip
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _PlantEn;
        }

        public bool IsPlantExists(Plants entityEn)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    return model.Plant.Any(p => p.company == entityEn.company && p.plant1 == entityEn.plant && (entityEn.id == 0 || p.id != entityEn.id));
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }
    }
}
