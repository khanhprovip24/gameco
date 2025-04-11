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
			boardButtons[oldX, oldY].BackColor = Color.White;

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
				{ 0, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 },
				{ 1, 1 }, { 1, -1 }, { -1, 1 }, { -1, -1 }
			};

			List<(int, int)> specialPoints = new List<(int, int)>
			{
				(0, 1), (0, 3), (1, 0), (1, 2), (1, 4),
				(2, 1), (2, 3), (3, 0), (3, 2), (3, 4),
				(4, 1), (4, 3)
			};

			// Nếu ở điểm đặc biệt => chỉ được đi ngang dọc
			bool isSpecial = specialPoints.Contains((piece.X, piece.Y));
			int limit = isSpecial ? 4 : 8;

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

		private int EvaluateMove(chess piece, int newX, int newY, chess[,] boardPieces)
		{
			int score = 0;

			// 1. Ưu tiên nước đi có thể ăn quân đối phương
			if (boardPieces[newX, newY] != null && boardPieces[newX, newY].Color != botColor)
			{
				score += 10; // Điểm cao hơn nếu ăn được quân đối phương
			}

			// 2. Ưu tiên nước đi gần trung tâm bàn cờ
			int centerX = 2, centerY = 2;
			score += 5 - (Math.Abs(newX - centerX) + Math.Abs(newY - centerY));

			// 3. Ưu tiên nước đi không để quân cờ bị bao vây
			if (!IsSurrounded(newX, newY, boardPieces))
			{
				score += 5;
			}

			return score;
		}

		private bool IsSurrounded(int x, int y, chess[,] boardPieces)
		{
			// Kiểm tra xem quân cờ có bị bao vây bởi quân đối phương hay không
			int[,] directions = {
		{ 0, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 }
	};

			for (int i = 0; i < directions.GetLength(0); i++)
			{
				int newX = x + directions[i, 0];
				int newY = y + directions[i, 1];

				if (newX >= 0 && newX < 5 && newY >= 0 && newY < 5)
				{
					if (boardPieces[newX, newY] == null || boardPieces[newX, newY].Color == botColor)
					{
						return false; // Không bị bao vây nếu có ô trống hoặc quân cờ cùng màu
					}
				}
			}

			return true; // Bị bao vây
		}

	}
}

