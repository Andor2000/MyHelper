namespace MyHelper
{
    public class BuildingScript
    {
        /// <summary>
        /// Название колонки.
        /// </summary>
        public static readonly string Table = @"MERGE {0} AS TARGET
USING (
    VALUES
        ";

        /// <summary>
        /// Сравнение\присвоение записей.
        /// </summary>
        public static readonly string Assign = @"TARGET.{0} = source.{0}";

        /// <summary>
        /// Колонки.
        /// </summary>
        public static readonly string Colomns = @"
) AS source ({0})
ON {1}
WHEN MATCHED THEN
    UPDATE SET
        {2}
WHEN NOT MATCHED THEN
    INSERT ({0})
    VALUES ({3});";

        /// <summary>
        /// Шаблон скрипта для Софтраст.
        /// </summary>
        public static readonly string TemplateScriptSoftrust = @"/* ----------------- Начало выполнения запроса ---------------------------------  */
------------------------ GO В ТЕЛЕ СКРИПТА ИСПОЛЬЗОВАТЬ НЕЛЬЗЯ, ЛИБО ЖЕ В ""В ПОДВАЛЕ"" ПРИСВОИТЬ СООТВЕТСТВУЮЩИЙ ГУИД, ОТНОСЯЩИЙСЯ К ДАННОМУ СКРИПТУ.

declare @scriptGuid uniqueidentifier,
        @taskNumber varchar(10),
        @scriptName varchar(150),
        @scriptdescription varchar(max)

set @scriptGuid = '{0}' -- для конкретного скрипта присваиваем значение нового гуида
set @taskNumber = '{1}' -- присваиваем номер задачи Jira
set @scriptName = '{2}' -- наименование скрипта (краткое описание)
set @scriptdescription = '{2}' -- описание скрипта

if (not exists(select 1 from web_script where [guid] = @scriptGuid))
begin
    insert into web_script(x_edition, x_status, [guid], taskNumber, Name, [description], RunCount, Success)
    select 1, 1, @scriptGuid, @taskNumber, @scriptName, @scriptdescription, 1, 0
end
else
begin
    update web_script
    set RunCount = RunCount + 1
    where [guid] = @scriptGuid
end

/* ----------------- end of: Начало выполнения запроса -------------------------  */

{3}

/* ----------------- Конец выполнения запроса ---------------------------------  */

update web_script
set Success = 1
where [guid] = @scriptGuid

/* ----------------- end of: Конец выполнения запроса -------------------------  */";
    }
}
