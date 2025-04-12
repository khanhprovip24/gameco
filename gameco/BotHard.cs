using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace gameco
{
	public class BotHard
	{
		private Random random = new Random();
		private Color botColor;

		public BotHard(Color color)
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

			List<(chess, (int, int))> bestMoves = new List<(chess, (int, int))>();
			List<(chess, (int, int))> normalMoves = new List<(chess, (int, int))>();

			foreach (var piece in botPieces)
			{
				List<(int, int)> validMoves = GetValidMoves(piece, boardPieces);

				foreach (var move in validMoves)
				{
					if (WouldCapture(piece.X, piece.Y, move.Item1, move.Item2, boardPieces))
					{
						bestMoves.Add((piece, move));
					}
					else
					{
						normalMoves.Add((piece, move));
					}
				}
			}

			if (bestMoves.Count > 0)
			{
				// Ưu tiên nước đi có thể ăn quân
				var (selectedPiece, move) = bestMoves[random.Next(bestMoves.Count)];
				MovePiece(selectedPiece.X, selectedPiece.Y, move.Item1, move.Item2, boardPieces, boardButtons);
			}
			else if (normalMoves.Count > 0)
			{
				// Nếu không ăn được ai, thì đi nước bình thường
				var (selectedPiece, move) = normalMoves[random.Next(normalMoves.Count)];
				MovePiece(selectedPiece.X, selectedPiece.Y, move.Item1, move.Item2, boardPieces, boardButtons);
			}
			// Nếu không có nước đi hợp lệ thì bỏ lượt
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

			int[,] directions = {
				{ 0, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 },
				{ 1, 1 }, { 1, -1 }, { -1, 1 }, { -1, -1 }
			};

			List<Point> specialPoints = new List<Point>
			{
				new Point(0, 1), new Point(0, 3),
				new Point(1, 0), new Point(1, 2), new Point(1, 4),
				new Point(2, 1), new Point(2, 3),
				new Point(3, 0), new Point(3, 2), new Point(3, 4),
				new Point(4, 1), new Point(4, 3)
			};

			bool isSpecialPoint = specialPoints.Contains(new Point(piece.X, piece.Y));
			int limit = isSpecialPoint ? 4 : 8;

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

		private bool WouldCapture(int oldX, int oldY, int newX, int newY, chess[,] boardPieces)
		{
			// Giả lập 1 nước đi để kiểm tra có ăn được quân không
			chess[,] tempBoard = (chess[,])boardPieces.Clone();

			// Di chuyển tạm thời
			tempBoard[newX, newY] = tempBoard[oldX, oldY];
			tempBoard[oldX, oldY] = null;

			// Gọi hàm kiểm tra Capture tạm (giống logic Capture)
			return CheckCapture(newX, newY, tempBoard);
		}

		private bool CheckCapture(int x, int y, chess[,] boardPieces)
		{
			Color myColor = boardPieces[x, y].Color;
			Color enemyColor = myColor == Color.Red ? Color.Blue : Color.Red;

			// Kiểm tra bị kẹp ngang hoặc dọc
			int[,] directions = { { 0, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 } };

			for (int i = 0; i < 4; i++)
			{
				int adjX = x + directions[i, 0];
				int adjY = y + directions[i, 1];
				int oppositeX = x + directions[i, 0] * 2;
				int oppositeY = y + directions[i, 1] * 2;

				if (adjX >= 0 && adjX < 5 && adjY >= 0 && adjY < 5 &&
					oppositeX >= 0 && oppositeX < 5 && oppositeY >= 0 && oppositeY < 5)
				{
					if (boardPieces[adjX, adjY] != null && boardPieces[oppositeX, oppositeY] != null)
					{
						if (boardPieces[adjX, adjY].Color == enemyColor && boardPieces[oppositeX, oppositeY].Color == myColor)
						{
							// Kẹp 2 bên, ăn được
							return true;
						}
					}
				}
			}

			return false;
		}
	}
}
