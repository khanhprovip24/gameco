using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace gameco
{
	public class BotNormal
	{
		private Random random = new Random();
		private Color botColor;

		public BotNormal(Color color)
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

			chess bestPiece = null;
			(int, int) bestMove = (-1, -1);
			int bestScore = int.MinValue;

			// 2. Duyệt qua tất cả các quân cờ của bot
			foreach (var piece in botPieces)
			{
				// 3. Tìm các nước đi hợp lệ
				List<(int, int)> validMoves = GetValidMoves(piece, boardPieces);

				foreach (var move in validMoves)
				{
					int score = EvaluateMove(piece, move.Item1, move.Item2, boardPieces);

					// 4. Ưu tiên nước đi có điểm số cao nhất
					if (score > bestScore)
					{
						bestScore = score;
						bestPiece = piece;
						bestMove = move;
					}
				}
			}

			// 5. Thực hiện nước đi tốt nhất
			if (bestPiece != null && bestMove != (-1, -1))
			{
				MovePiece(bestPiece.X, bestPiece.Y, bestMove.Item1, bestMove.Item2, boardPieces, boardButtons);
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

		private int EvaluateMove(chess piece, int newX, int newY, chess[,] boardPieces)
		{
			int score = 0;

			// 1. Ưu tiên nước đi có thể ăn quân đối phương
			if (boardPieces[newX, newY] != null && boardPieces[newX, newY].Color != botColor)
			{
				score += 20; // Điểm cao hơn nếu ăn được quân đối phương
			}

			// 2. Tránh nước đi dẫn đến bị ăn
			if (IsMoveDangerous(newX, newY, boardPieces))
			{
				score -= 15; // Trừ điểm nếu nước đi có thể bị ăn
			}

			// 3. Ưu tiên nước đi gần trung tâm bàn cờ
			int centerX = 2, centerY = 2;
			score += 5 - (Math.Abs(newX - centerX) + Math.Abs(newY - centerY));

			return score;
		}

		private bool IsMoveDangerous(int x, int y, chess[,] boardPieces)
		{
			Color enemyColor = botColor == Color.White ? Color.Black : Color.White;

			// Kiểm tra các ô xung quanh xem có quân đối phương không
			int[,] directions = {
				{ 0, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 },
				{ 1, 1 }, { 1, -1 }, { -1, 1 }, { -1, -1 }
			};

			for (int i = 0; i < directions.GetLength(0); i++)
			{
				int newX = x + directions[i, 0];
				int newY = y + directions[i, 1];

				if (newX >= 0 && newX < 5 && newY >= 0 && newY < 5)
				{
					if (boardPieces[newX, newY] != null && boardPieces[newX, newY].Color == enemyColor)
					{
						return true; // Có nguy cơ bị ăn
					}
				}
			}

			return false;
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
