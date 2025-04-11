using System.Drawing;
using System;
using System.Collections.Generic;

namespace gameco
{
	public class chess
	{
		public int X { get; set; }
		public int Y { get; set; }
		public Color Color { get; set; }

		public chess(int x, int y, Color color)
		{
			if (color != Color.Blue && color != Color.Red)
			{
				throw new ArgumentException("Only blue and red colors are allowed.");
			}

			X = x;
			Y = y;
			Color = color;
		}

		public void Move(int newX, int newY, chess[,] boardPieces)
		{
			boardPieces[newX, newY] = this;
			boardPieces[X, Y] = null;
			X = newX;
			Y = newY;
		}

		public void Capture(int x, int y, chess[,] boardPieces)
		{
			// Các điểm bị hạn chế
			List<Point> restrictedPoints = new List<Point>
			{
				new Point(0, 1), new Point(0, 3),
				new Point(1, 0), new Point(1, 2), new Point(1, 4),
				new Point(2, 1), new Point(2, 3),
				new Point(3, 0), new Point(3, 2), new Point(3, 4),
				new Point(4, 1), new Point(4, 3)
			};

			// Các hướng hợp lệ theo đường kẻ bàn cờ (ngang, dọc, và các đường chéo hợp lệ)
			int[,] validDirectionsStraight = {
				{ 0, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 }  // Ngang, dọc
            };

			int[,] validDirectionsDiagonal = {
				{ 1, 1 }, { -1, -1 }, { 1, -1 }, { -1, 1 } // Đường chéo hợp lệ
            };

			int[,] validDirections;

			// Kiểm tra xem điểm hiện tại có nằm trong danh sách các điểm bị hạn chế hay không
			if (restrictedPoints.Contains(new Point(x, y)))
			{
				validDirections = validDirectionsStraight;
			}
			else
			{
				validDirections = new int[validDirectionsStraight.GetLength(0) + validDirectionsDiagonal.GetLength(0), 2];
				Array.Copy(validDirectionsStraight, 0, validDirections, 0, validDirectionsStraight.Length);
				Array.Copy(validDirectionsDiagonal, 0, validDirections, validDirectionsStraight.Length, validDirectionsDiagonal.Length);
			}

			for (int i = 0; i < validDirections.GetLength(0); i++)
			{
				int dx = validDirections[i, 0];
				int dy = validDirections[i, 1];

				int prevX = x - dx;
				int prevY = y - dy;
				int nextX = x + dx;
				int nextY = y + dy;

				// Kiểm tra biên và đảm bảo có 3 quân theo hướng hợp lệ
				if (IsValidMove(prevX, prevY) && IsValidMove(nextX, nextY))
				{
					// Kiểm tra điều kiện "gánh"
					if (boardPieces[prevX, prevY] != null &&
						boardPieces[nextX, nextY] != null &&
						boardPieces[prevX, prevY].Color != this.Color &&
						boardPieces[nextX, nextY].Color != this.Color)
					{
						// Đổi màu cả hai quân cờ bị gánh
						boardPieces[prevX, prevY].Color = this.Color;
						boardPieces[nextX, nextY].Color = this.Color;

						// Cập nhật giao diện
						Form1.boardButtons[prevX, prevY].BackColor = this.Color;
						Form1.boardButtons[nextX, nextY].BackColor = this.Color;
					}
				}
			}
		}

		public void CaptureSurroundedPieces(chess[,] boardPieces)
		{
			bool[,] visited = new bool[5, 5];

			for (int i = 0; i < 5; i++)
			{
				for (int j = 0; j < 5; j++)
				{
					if (boardPieces[i, j] != null && boardPieces[i, j].Color != this.Color && !visited[i, j])
					{
						List<(int, int)> region = new List<(int, int)>();
						Color enemyColor = boardPieces[i, j].Color;

						if (IsSurrounded(boardPieces, i, j, enemyColor, visited, region))
						{
							foreach (var (px, py) in region)
							{
								boardPieces[px, py].Color = this.Color;
								Form1.boardButtons[px, py].BackColor = this.Color;
							}
						}
					}
				}
			}
		}

		// Kiểm tra xem một nhóm quân có bị bao vây hay không
		private bool IsSurrounded(chess[,] board, int x, int y, Color enemyColor, bool[,] visited, List<(int, int)> region)
		{
			// Các hướng bao gồm cả 4 hướng ngang dọc và 4 hướng chéo
			int[,] directions = { { 0, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 },  // ngang dọc
                          { 1, 1 }, { 1, -1 }, { -1, -1 }, { -1, 1 } }; // chéo

			// Kiểm tra các điểm đặc biệt, chỉ có 4 hướng ngang dọc
			List<(int, int)> specialPoints = new List<(int, int)>
	{
		(0, 1), (0, 3), (1, 0), (1, 2), (1, 4),
		(2, 1), (2, 3), (3, 0), (3, 2), (3, 4),
		(4, 1), (4, 3)
	};

			// Nếu điểm (x, y) là một trong các điểm đặc biệt, chỉ sử dụng các hướng ngang dọc
			if (specialPoints.Contains((x, y)))
			{
				directions = new int[,] { { 0, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 } }; // chỉ có 4 hướng ngang dọc
			}

			Queue<(int, int)> queue = new Queue<(int, int)>();
			queue.Enqueue((x, y));
			region.Add((x, y));
			visited[x, y] = true;
			bool surrounded = true;

			while (queue.Count > 0)
			{
				var (cx, cy) = queue.Dequeue();

				// Kiểm tra các quân cờ đối thủ xung quanh
				bool hasPath = false;
				for (int i = 0; i < directions.GetLength(0); i++)
				{
					int nx = cx + directions[i, 0];
					int ny = cy + directions[i, 1];

					// Kiểm tra biên để tránh vượt quá phạm vi bàn cờ
					if (nx < 0 || nx > 4 || ny < 0 || ny > 4)
					{
						continue; // Không kiểm tra ngoài phạm vi bàn cờ
					}

					// Nếu có ô trống cạnh quân cờ đối thủ, có thể di chuyển, quân cờ không bị vây
					if (board[nx, ny] == null)
					{
						hasPath = true;
						break;
					}

					// Nếu là quân đối thủ chưa kiểm tra, thêm vào danh sách
					if (!visited[nx, ny] && board[nx, ny].Color == enemyColor)
					{
						visited[nx, ny] = true;
						queue.Enqueue((nx, ny));
						region.Add((nx, ny));
					}
				}

				// Nếu quân cờ không có lối đi hợp lệ thì sẽ bị bao vây
				if (hasPath)
				{
					surrounded = false;
				}
			}

			return surrounded;
		}


		private bool IsValidMove(int x, int y)
		{
			return x >= 0 && x < 5 && y >= 0 && y < 5;
		}
	}
}
