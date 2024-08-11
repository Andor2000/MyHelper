using MyHelper.Dto;
using MyHelper.Models.Entity;

namespace MyHelper.Extensions
{
    /// <summary>
    /// Маппинг моделей.    
    /// </summary>
    public static class MappingProfiller
    {
        /// <summary>
        /// Маппинг модели таблицы из entity в dto.
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="entity"></param>
        public static TableEntity MapTableDtoToEntity(this TableEntity entity, TableDto dto)
        {
            entity.Sort = dto.Sort;
            entity.Name = dto.TextBox.Text;
            entity.IsTemplateScript = dto.IsTemplateScript;
            entity.Guid = dto.SaveScriptModel.Guid.Text;
            entity.Task = dto.SaveScriptModel.Task.Text;
            entity.Sprint = dto.SaveScriptModel.Sprint.Text;
            entity.Number = dto.SaveScriptModel.Number.Text;
            entity.Project = dto.SaveScriptModel.Project.Text;
            entity.Description = dto.SaveScriptModel.Description.Text;
            return entity;
        }

        /// <summary>
        /// Маппинг модели таблицы из entity в dto.
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="entity"></param>
        public static TableDto MapTableEntityToDto(this TableDto dto, TableEntity entity)
        {
            dto.Id = entity.Id;
            dto.IsTemplateScript = entity.IsTemplateScript;
            dto.Sort = entity.Sort;
            dto.TextBox.Text = entity.Name;

            dto.SaveScriptModel.Guid.Text = entity.Guid;
            dto.SaveScriptModel.Description.Text = entity.Description;
            dto.SaveScriptModel.Number.Text = entity.Number;
            dto.SaveScriptModel.Project.Text = entity.Project;
            dto.SaveScriptModel.Sprint.Text = entity.Sprint;
            dto.SaveScriptModel.Task.Text = entity.Task;
            return dto;
        }

        /// <summary>
        /// Маппинг модели колонки из entity в dto.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dto"></param>
        public static ColomnDto MapColomnEntityToDto(this ColomnDto dto, ColomnEntity entity)
        {
            dto.Id = entity.Id;
            dto.Sort = entity.Sort;
            dto.Records = entity.Records;
            dto.IsQuotes = entity.IsQuotes;
            dto.IsEqualsRecordStar = entity.IsEqualsRecordStar;
            dto.TableId = entity.TableId;
            dto.TextBox.Text = entity.Name;
            dto.TextBoxCount.Text = dto.CountRecords;
            return dto;
        }

        /// <summary>
        /// Маппинг модели колонки из dto в entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dto"></param>
        public static ColomnEntity MapColomnDtoToEntity(this ColomnEntity entity, ColomnDto dto)
        {
            entity.Sort = dto.Sort;
            entity.Records = dto.Records;
            entity.Name = dto.TextBox.Text;
            entity.IsQuotes = dto.IsQuotes;
            entity.IsEqualsRecordStar = dto.IsEqualsRecordStar;
            return entity;
        }
    }
}
