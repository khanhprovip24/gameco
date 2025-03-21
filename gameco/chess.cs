using System;
using System.Drawing;

namespace gameco
{
	internal class chess
	{
		public int X { get; set; }
		public int Y { get; set; }
		public Color Color { get; set; }

		public chess(int x, int y, Color color)
		{
			X = x;
			Y = y;
			Color = color;
		}

		public bool IsValidMove(int newX, int newY, chess[,] board)
		{
			if (newX < 0 || newX >= board.GetLength(0) || newY < 0 || newY >= board.GetLength(1))
				return false;

			if (board[newX, newY] != null && board[newX, newY].Color == this.Color)
				return false;

			return true;
		}

		public void Move(int newX, int newY, chess[,] board)
		{
			if (IsValidMove(newX, newY, board))
			{
				board[newX, newY] = this;
				board[X, Y] = null;
				X = newX;
				Y = newY;

				between(newX, newY, board);
				block(newX, newY, board);
			}
		}

		public void between(int x, int y, chess[,] board)
		{
			int[,] directions = new int[,]
			{
				{ 0, 1 },  { 1, 0 },  { 0, -1 }, { -1, 0 },
				{ 1, 1 },  { 1, -1 }, { -1, 1 }, { -1, -1 }
			};

			for (int i = 0; i < directions.GetLength(0); i++)
			{
				int dx = directions[i, 0];
				int dy = directions[i, 1];

				int nx = x + dx;
				int ny = y + dy;
				int nnx = x + 2 * dx;
				int nny = y + 2 * dy;

				if (nnx >= 0 && nnx < board.GetLength(0) && nny >= 0 && nny < board.GetLength(1))
				{
					if (board[nx, ny] != null && board[nnx, nny] != null &&
						board[nx, ny].Color != this.Color && board[nnx, nny].Color == this.Color)
					{
						board[nx, ny] = null;
					}
				}
			}
		}

		public void block(int x, int y, chess[,] board)
		{
			// Implement the logic for blocking pieces
		}
	}
}
