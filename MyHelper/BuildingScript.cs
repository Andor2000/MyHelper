using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHelper
{
    public class BuildingScript
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly string Table = @"MERGE {0} AS TARGET
USING (
	VALUES
        ";

        /// <summary>
        /// 
        /// </summary>
        public static readonly string Assign = @"        TARGET.{0} = source.{0}";
        /// <summary>
        /// 
        /// </summary>
        public static readonly string Colomns = @"
) AS source ({0})
ON TARGET.ReactionGUID = source.ReactionGUID
WHEN MATCHED THEN
    UPDATE SET
{1}
WHEN NOT MATCHED THEN
    INSERT ({0})
    VALUES ({2});";

    }
}
