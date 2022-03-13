using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.Lextatico.Application.Dtos.Filter
{
    public class PaginationFilterDto
    {
        private int _page;
        private int _size;

        public int Page
        {
            get => _page;
            set => _page = value < 1 ? 1 : value;
        }

        public int Size
        {
            get => _size;
            set => _size = value < 1 ? 1 : value;
        }

        public PaginationFilterDto()
        {
            Page = 1;
            Size = 10;
        }
        public PaginationFilterDto(int page, int size)
        {
            Page = page;
            Size = size;
        }
    }
}
