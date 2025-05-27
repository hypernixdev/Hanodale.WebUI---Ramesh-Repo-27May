using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanodale.Entity.Core;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.Xml;
using System.ServiceModel;
using System.Data.Objects.SqlClient;
using System.Collections;
using System.Globalization;
using Hanodale.Domain.DTOs;
using Hanodale.DataAccessLayer.Interfaces;
using Hanodale.Domain;

namespace Hanodale.DataAccessLayer.Services
{
    public class BaseService
    {
        public object GetAttribute(object entity, string _name)
        {
            return entity.GetType().GetProperty(_name).GetValue(entity, null);
        }

        public string GetResourceName(object entity, string _name)
        {
            object res = GetAttribute(entity, _name);

            if (res == null)
            {
                res = GetAttribute(entity, "en-US");
                if (res == null)
                {
                    return _name;
                }
            }

            return res.ToString();
        }

        public List<TableProfileMetadatas> GetFieldMetadata(TableProfiles entityEn)
        {
            var lst = new List<TableProfileMetadatas>();
            try
            {
                //bool hasCulture=false;
                //if (!string.IsNullOrEmpty(entityEn.culture))
                //{
                //    hasCulture = true;
                //}
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var obj = model.TableProfiles.ToList().SingleOrDefault(p => p.menu_Id == entityEn.menu_Id && (entityEn.parentMenu_Id==null || (p.parent_Id != null && p.TableProfile2.menu_Id == entityEn.parentMenu_Id)) && p.name == entityEn.name);
                    if (obj != null)
                    {
                        if (obj.TableProfileMetadatas != null)
                        {
                            foreach (var item in obj.TableProfileMetadatas)
                            {
                                var metadata = new TableProfileMetadatas();
                                metadata.id = item.id;
                                metadata.fieldName = item.fieldName;
                                metadata.valueFieldName = item.valueFieldName;
                                metadata.isEditableInCreate = item.isEditableInCreate;
                                metadata.isEditableInEdit = item.isEditableInEdit;
                                metadata.isMandatory = item.isMandatory;
                                metadata.sortOrder = item.sortOrder;
                                metadata.templateName = item.templateName;
                                metadata.resourceKeyNameForGridView = item.LocalizationResource.keyName;
                                //metadata.resourceNameForGridView = item.LocalizationResource.defaultValue;
                                //if (hasCulture)
                                //{
                                //    var _culture = item.LocalizationResource.LocalizationLanguageResources.FirstOrDefault(p => p.LocalizationLanguage.culture == entityEn.culture);
                                //    if (_culture != null)
                                //    {
                                //        metadata.resourceNameForGridView = _culture.LocalizationResource.defaultValue;
                                //    }
                                //}
                                metadata.validationRule = item.validationRule;
                                metadata.visibility = item.visibility;
                                metadata.visibilityInCreate = item.visibilityInCreate;
                                metadata.visibilityInEdit = item.visibilityInEdit;
                                metadata.visibilityInGridView = item.visibilityInGridView;

                                if (item.TableProfileMetadataBool != null)
                                {
                                    metadata.metadataBool = new TableProfileMetadataBools();
                                    metadata.metadataBool.selectionOption = item.TableProfileMetadataBool.selectionOption;
                                    metadata.metadataBool.defaultValue = item.TableProfileMetadataBool.defaultValue;
                                }
                                else if (item.TableProfileMetadataDateTime != null)
                                {
                                    metadata.metadataDateTime = new TableProfileMetadataDateTimes();
                                    metadata.metadataDateTime.maxDate = item.TableProfileMetadataDateTime.maxDate;
                                    metadata.metadataDateTime.minDate = item.TableProfileMetadataDateTime.minDate;
                                    metadata.metadataDateTime.isAllowPastDate = item.TableProfileMetadataDateTime.isAllowPastDate;
                                    metadata.metadataDateTime.isCurrentDateDefault = item.TableProfileMetadataDateTime.isCurrentDateDefault;
                                    metadata.metadataDateTime.isFollowCalendarSetting = item.TableProfileMetadataDateTime.isFollowCalendarSetting;
                                }
                                else if (item.TableProfileMetadataDecimal != null)
                                {
                                    metadata.metadataDecimal = new TableProfileMetadataDecimals();
                                    metadata.metadataDecimal.maxValue = item.TableProfileMetadataDecimal.maxValue;
                                    metadata.metadataDecimal.minValue = item.TableProfileMetadataDecimal.minValue;
                                    metadata.metadataDecimal.defaultValue = item.TableProfileMetadataDecimal.defaultValue;
                                }

                                else if (item.TableProfileMetadataInteger != null)
                                {
                                    metadata.metadataInteger = new TableProfileMetadataIntegers();
                                    metadata.metadataInteger.maxValue = item.TableProfileMetadataInteger.maxValue;
                                    metadata.metadataInteger.minValue = item.TableProfileMetadataInteger.minValue;
                                    metadata.metadataInteger.defaultValue = item.TableProfileMetadataInteger.defaultValue;
                                }

                                else if (item.TableProfileMetadataList != null)
                                {
                                    metadata.metadataList = new TableProfileMetadataLists();
                                    metadata.metadataList.isMultiValue = item.TableProfileMetadataList.isMultiValue;
                                    metadata.metadataList.lookDefaultValue = item.TableProfileMetadataList.lookDefaultValue;
                                    metadata.metadataList.lookUpValue = item.TableProfileMetadataList.lookUpValue;
                                    if (item.TableProfileMetadataList.parent_Id != null)
                                    {
                                        metadata.metadataList.hasParent = true;
                                        metadata.metadataList.parentFieldName = item.TableProfileMetadataList.TableProfileMetadataList2.TableProfileMetadata.fieldName;
                                        metadata.metadataList.dataRetrieveURL = item.TableProfileMetadataList.dataRetrieveURL;
                                    }

                                    metadata.metadataList.lstListValue = new List<TableProfileMetadataListValues>();

                                    if (item.TableProfileMetadataList.TableProfileMetadataListValues != null)
                                    {
                                        foreach (var listItem in item.TableProfileMetadataList.TableProfileMetadataListValues)
                                        {
                                            var dropdownItem = new TableProfileMetadataListValues();
                                            dropdownItem.id = listItem.id;
                                            dropdownItem.fieldText = listItem.fieldText;

                                            metadata.metadataList.lstListValue.Add(dropdownItem);
                                        }
                                    }
                                }
                                else if (item.TableProfileMetadataText != null)
                                {
                                    metadata.metadataText = new TableProfileMetadataTexts();
                                    metadata.metadataText.maxLength = item.TableProfileMetadataText.maxLength;
                                    metadata.metadataText.minLength = item.TableProfileMetadataText.minLength;
                                    metadata.metadataText.defaultValue = item.TableProfileMetadataText.defaultValue;
                                }

                                lst.Add(metadata);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }

            return lst;
        }

        public TableProfiles GetTableProfileWithTabs(TableProfiles entityEn)
        {
            var result = new TableProfiles();
            result.lstTableProfileTabs = new List<TableProfiles>();
            try
            {
                
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var obj = model.TableProfiles.FirstOrDefault(p => p.menu_Id == entityEn.menu_Id && p.parent_Id == null);
                    if (obj != null)
                    {
                        result.id = obj.id;
                        result.menu_Id = obj.menu_Id ?? 0;
                        result.name = obj.name;
                        result.parentMenu_Id = obj.parent_Id;
                        result.sortOrder = obj.sortOrder;
                        result.url = obj.url;
                        result.visibility = obj.visibility;
                        result.icon = obj.icon;
                        result.resourceNameKey = obj.LocalizationResource.keyName;

                        if (obj.TableProfile1 != null)
                        {
                            foreach (var item in obj.TableProfile1)
                            {
                                var tab = new TableProfiles();
                                tab.id = item.id;
                                tab.menu_Id = item.menu_Id??0;
                                tab.name = item.name;
                                tab.parentMenu_Id = item.parent_Id;
                                tab.sortOrder = item.sortOrder;
                                tab.url = item.url;
                                tab.visibility = item.visibility;
                                tab.icon = item.icon;
                                tab.resourceNameKey = item.LocalizationResource.keyName;

                                result.lstTableProfileTabs.Add(tab);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }

            return result;
        }
    }
}
