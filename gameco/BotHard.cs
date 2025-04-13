using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace gameco
{
	public class BotHard
	{
		private Color botColor;
		private Color opponentColor;
		private const int MAX_DEPTH = 3; // Độ sâu tối đa của thuật toán Minimax

		public BotHard(Color color)
		{
			botColor = color;
			opponentColor = botColor == Color.Red ? Color.Blue : Color.Red;
		}

		public void MakeMove(chess[,] boardPieces, Button[,] boardButtons)
		{
			int bestScore = int.MinValue;
			(chess, (int, int)) bestMove = (null, (-1, -1));

			// Tìm tất cả quân cờ của bot
			List<chess> botPieces = GetBotPieces(boardPieces);

			// Duyệt qua tất cả các quân cờ và các nước đi hợp lệ
			foreach (var piece in botPieces)
			{
				List<(int, int)> validMoves = GetValidMoves(piece, boardPieces);

				foreach (var move in validMoves)
				{
					// Giả lập nước đi
					chess[,] tempBoard = CloneBoard(boardPieces);
					SimulateMove(tempBoard, piece.X, piece.Y, move.Item1, move.Item2);

					// Tính điểm của nước đi bằng Minimax
					int score = Minimax(tempBoard, MAX_DEPTH, false, int.MinValue, int.MaxValue);

					// Cập nhật nước đi tốt nhất
					if (score > bestScore)
					{
						bestScore = score;
						bestMove = (piece, move);
					}
				}
			}

			// Thực hiện nước đi tốt nhất
			if (bestMove.Item1 != null && bestMove.Item2 != (-1, -1))
			{
				MovePiece(bestMove.Item1.X, bestMove.Item1.Y, bestMove.Item2.Item1, bestMove.Item2.Item2, boardPieces, boardButtons);
			}
		}

		private int Minimax(chess[,] boardPieces, int depth, bool isMaximizing, int alpha, int beta)
		{
			// Điều kiện dừng: đạt độ sâu tối đa hoặc không còn nước đi
			if (depth == 0 || IsGameOver(boardPieces))
			{
				return EvaluateBoard(boardPieces);
			}

			if (isMaximizing)
			{
				int maxEval = int.MinValue;
				List<chess> botPieces = GetBotPieces(boardPieces);

				foreach (var piece in botPieces)
				{
					List<(int, int)> validMoves = GetValidMoves(piece, boardPieces);

					foreach (var move in validMoves)
					{
						// Giả lập nước đi
						chess[,] tempBoard = CloneBoard(boardPieces);
						SimulateMove(tempBoard, piece.X, piece.Y, move.Item1, move.Item2);

						// Đệ quy Minimax
						int eval = Minimax(tempBoard, depth - 1, false, alpha, beta);
						maxEval = Math.Max(maxEval, eval);
						alpha = Math.Max(alpha, eval);

						// Cắt tỉa alpha-beta
						if (beta <= alpha)
							break;
					}
				}

				return maxEval;
			}
			else
			{
				int minEval = int.MaxValue;
				List<chess> opponentPieces = GetOpponentPieces(boardPieces);

				foreach (var piece in opponentPieces)
				{
					List<(int, int)> validMoves = GetValidMoves(piece, boardPieces);

					foreach (var move in validMoves)
					{
						// Giả lập nước đi
						chess[,] tempBoard = CloneBoard(boardPieces);
						SimulateMove(tempBoard, piece.X, piece.Y, move.Item1, move.Item2);

						// Đệ quy Minimax
						int eval = Minimax(tempBoard, depth - 1, true, alpha, beta);
						minEval = Math.Min(minEval, eval);
						beta = Math.Min(beta, eval);

						// Cắt tỉa alpha-beta
						if (beta <= alpha)
							break;
					}
				}

				return minEval;
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
			// Kiểm tra xem tọa độ có nằm trong bàn cờ không
			if (x < 0 || x >= 5 || y < 0 || y >= 5)
				return false;

			// Kiểm tra xem ô có trống không
			return boardPieces[x, y] == null;
		}

		private int EvaluateBoard(chess[,] boardPieces)
		{
			int score = 0;

			// Tính điểm dựa trên số lượng quân cờ
			foreach (var piece in boardPieces)
			{
				if (piece != null)
				{
					if (piece.Color == botColor)
					{
						score += 10; // Điểm cho quân cờ của bot
					}
					else if (piece.Color == opponentColor)
					{
						score -= 10; // Trừ điểm cho quân cờ của đối thủ
					}
				}
			}

			// Ưu tiên vị trí gần trung tâm
			int centerX = 2, centerY = 2;
			for (int i = 0; i < 5; i++)
			{
				for (int j = 0; j < 5; j++)
				{
					if (boardPieces[i, j] != null && boardPieces[i, j].Color == botColor)
					{
						score += 5 - (Math.Abs(i - centerX) + Math.Abs(j - centerY));
					}
				}
			}

			return score;
		}

		private void SimulateMove(chess[,] boardPieces, int oldX, int oldY, int newX, int newY)
		{
			boardPieces[newX, newY] = boardPieces[oldX, oldY];
			boardPieces[oldX, oldY] = null;
		}

		private List<chess> GetBotPieces(chess[,] boardPieces)
		{
			List<chess> botPieces = new List<chess>();
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
			return botPieces;
		}

		private List<chess> GetOpponentPieces(chess[,] boardPieces)
		{
			List<chess> opponentPieces = new List<chess>();
			for (int i = 0; i < 5; i++)
			{
				for (int j = 0; j < 5; j++)
				{
					if (boardPieces[i, j] != null && boardPieces[i, j].Color == opponentColor)
					{
						opponentPieces.Add(boardPieces[i, j]);
					}
				}
			}
			return opponentPieces;
		}

		private chess[,] CloneBoard(chess[,] boardPieces)
		{
			return (chess[,])boardPieces.Clone();
		}

		private bool IsGameOver(chess[,] boardPieces)
		{
			// Kiểm tra nếu không còn quân cờ của một bên
			bool botExists = false, opponentExists = false;

			foreach (var piece in boardPieces)
			{
				if (piece != null)
				{
					if (piece.Color == botColor)
						botExists = true;
					else if (piece.Color == opponentColor)
						opponentExists = true;
				}
			}

			return !botExists || !opponentExists;
		}
	}
}
