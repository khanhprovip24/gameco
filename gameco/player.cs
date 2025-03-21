using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameco
{
	public class Player
	{
		public string Name { get; set; }
		public int PlayerID { get; set; } // 1: Đen, 2: Trắng

		public Player(string name, int id)
		{
			Name = name;
			PlayerID = id;
		}
	}

}
