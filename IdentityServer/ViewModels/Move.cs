using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace IdentityServer.ViewModels
{
    public class Move
    {
        public class Movie
        {
            public int Id { get; set; }
            public string Title { get; set; }

            /// <summary>
            /// 仅显示日期，而非时间信息。
            /// </summary>
            [DataType(DataType.Date)]
            public DateTime ReleaseDate { get; set; }
            public string Genre { get; set; }
            public decimal Price { get; set; }
        }
    }
}
