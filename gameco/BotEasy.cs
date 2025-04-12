using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace gameco
{
	public class BotEasy
	{
		private Random random = new Random();
		private Color botColor;

		public BotEasy(Color color)
		{
			botColor = color;
		}

		public void MakeMove(chess[,] boardPieces, Button[,] boardButtons)
		{
			List<chess> botPieces = new List<chess>();

			// 1. Tìm tất cả quân cờ thuộc về bot
			for (int i = 0; i < 5; i++)
			{
				for (int j = 0; j < 5; j++)
				{
					if (boardPieces[i, j] != null && boardPieces[i, j].Color == botColor)
					{
						botPieces.Add(boardPieces[i, j]);
					}
				}
			}

			// Nếu không còn quân, thua
			if (botPieces.Count == 0)
				return;

			while (botPieces.Count > 0)
			{
				// 2. Chọn 1 quân cờ ngẫu nhiên
				int randomIndex = random.Next(botPieces.Count);
				chess selected = botPieces[randomIndex];

				// 3. Tìm các nước đi hợp lệ
				List<(int, int)> validMoves = GetValidMoves(selected, boardPieces);

				if (validMoves.Count > 0)
				{
					// 4. Chọn 1 nước đi ngẫu nhiên
					var (newX, newY) = validMoves[random.Next(validMoves.Count)];

					// 5. Di chuyển quân cờ trên bàn cờ
					MovePiece(selected.X, selected.Y, newX, newY, boardPieces, boardButtons);

					break; // Đã đi 1 lần thì kết thúc lượt
				}
				else
				{
					// Nếu quân này không di chuyển được, bỏ và thử quân khác
					botPieces.RemoveAt(randomIndex);
				}
			}
		}

		private void MovePiece(int oldX, int oldY, int newX, int newY, chess[,] boardPieces, Button[,] boardButtons)
		{
			// Cập nhật trạng thái của Button
			boardButtons[newX, newY].BackColor = boardButtons[oldX, oldY].BackColor;
			boardButtons[newX, newY].Visible = true; // Hiển thị ô mới sau khi di chuyển
			boardButtons[oldX, oldY].BackColor = Color.White;
			boardButtons[oldX, oldY].Visible = false; // Ẩn ô cũ sau khi di chuyển

			// Cập nhật trạng thái của chess
			if (boardPieces[oldX, oldY] != null)
			{
				boardPieces[newX, newY] = boardPieces[oldX, oldY];
				boardPieces[oldX, oldY] = null;
				boardPieces[newX, newY].Move(newX, newY, boardPieces);
				boardPieces[newX, newY].Capture(newX, newY, boardPieces);
				boardPieces[newX, newY].CaptureSurroundedPieces(boardPieces);
			}
		}

		private List<(int, int)> GetValidMoves(chess piece, chess[,] boardPieces)
		{
			List<(int, int)> moves = new List<(int, int)>();

			// Các hướng đi hợp lệ (trái, phải, lên, xuống, chéo)
			int[,] directions = {
				{ 0, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 }, // Dọc và ngang
				{ 1, 1 }, { 1, -1 }, { -1, 1 }, { -1, -1 } // Chéo
			};

			// Các điểm đặc biệt chỉ được đi dọc và ngang
			List<Point> specialPoints = new List<Point>
			{
				new Point(0, 1), new Point(0, 3),
				new Point(1, 0), new Point(1, 2), new Point(1, 4),
				new Point(2, 1), new Point(2, 3),
				new Point(3, 0), new Point(3, 2), new Point(3, 4),
				new Point(4, 1), new Point(4, 3)
			};

			// Kiểm tra xem quân cờ có ở điểm đặc biệt không
			bool isSpecialPoint = specialPoints.Contains(new Point(piece.X, piece.Y));
			int limit = isSpecialPoint ? 4 : 8; // Nếu ở điểm đặc biệt, chỉ duyệt 4 hướng (dọc và ngang)

			for (int i = 0; i < limit; i++)
			{
				int newX = piece.X + directions[i, 0];
				int newY = piece.Y + directions[i, 1];

				if (IsValidMove(newX, newY, boardPieces))
				{
					moves.Add((newX, newY));
				}
			}

			return moves;
		}

		private bool IsValidMove(int x, int y, chess[,] boardPieces)
		{
			return x >= 0 && x < 5 && y >= 0 && y < 5 && boardPieces[x, y] == null;
		}
	}
}
