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

		public void MakeMove(System.Windows.Forms.Button[,] boardButtons, chess[,] boardPieces)
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

		private void MovePiece(int oldX, int oldY, int newX, int newY, chess[,] boardPieces, System.Windows.Forms.Button[,] boardButtons)
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



		private (int, int, chess) FindBestMove(chess[,] boardPieces)
		{
			int bestScore = int.MinValue;
			(int bestX, int bestY) = (-1, -1);
			chess bestPiece = null;

			// Tìm tất cả quân cờ của bot
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

			// Duyệt qua tất cả các quân cờ và các nước đi hợp lệ
			foreach (var piece in botPieces)
			{
				List<(int, int)> validMoves = GetValidMoves(piece, boardPieces);

				foreach (var (newX, newY) in validMoves)
				{
					// Tạo bản sao bàn cờ để mô phỏng nước đi
					chess[,] boardCopy = CloneBoard(boardPieces);
					chess pieceCopy = boardCopy[piece.X, piece.Y];
					pieceCopy.Move(newX, newY, boardCopy);

					// Tính điểm của trạng thái bàn cờ sau nước đi
					int score = Minimax(boardCopy, 3, false);

					// Cập nhật nước đi tốt nhất
					if (score > bestScore)
					{
						bestScore = score;
						bestX = newX;
						bestY = newY;
						bestPiece = piece;
					}
				}
			}

			return (bestX, bestY, bestPiece);
		}

		private int Minimax(chess[,] board, int depth, bool isMaximizing)
		{
			// Điều kiện dừng: đạt độ sâu tối đa hoặc kết thúc trò chơi
			if (depth == 0 || IsGameOver(board))
			{
				return EvaluateBoard(board);
			}

			if (isMaximizing)
			{
				int maxEval = int.MinValue;

				// Tìm tất cả quân cờ của bot
				List<chess> botPieces = GetPiecesByColor(board, botColor);

				foreach (var piece in botPieces)
				{
					List<(int, int)> validMoves = GetValidMoves(piece, board);

					foreach (var (newX, newY) in validMoves)
					{
						// Mô phỏng nước đi
						chess[,] boardCopy = CloneBoard(board);
						chess pieceCopy = boardCopy[piece.X, piece.Y];
						pieceCopy.Move(newX, newY, boardCopy);

						// Đệ quy Minimax
						int eval = Minimax(boardCopy, depth - 1, false);
						maxEval = Math.Max(maxEval, eval);
					}
				}

				return maxEval;
			}
			else
			{
				int minEval = int.MaxValue;

				// Tìm tất cả quân cờ của đối thủ
				Color enemyColor = botColor == Color.White ? Color.Black : Color.White;
				List<chess> enemyPieces = GetPiecesByColor(board, enemyColor);

				foreach (var piece in enemyPieces)
				{
					List<(int, int)> validMoves = GetValidMoves(piece, board);

					foreach (var (newX, newY) in validMoves)
					{
						// Mô phỏng nước đi
						chess[,] boardCopy = CloneBoard(board);
						chess pieceCopy = boardCopy[piece.X, piece.Y];
						pieceCopy.Move(newX, newY, boardCopy);

						// Đệ quy Minimax
						int eval = Minimax(boardCopy, depth - 1, true);
						minEval = Math.Min(minEval, eval);
					}
				}

				return minEval;
			}
		}

		private int EvaluateBoard(chess[,] board)
		{
			// Hàm đánh giá trạng thái bàn cờ (ví dụ: số lượng quân cờ)
			int score = 0;

			for (int i = 0; i < 5; i++)
			{
				for (int j = 0; j < 5; j++)
				{
					if (board[i, j] != null)
					{
						score += board[i, j].Color == botColor ? 1 : -1;
					}
				}
			}

			return score;
		}

		private bool IsGameOver(chess[,] board)
		{
			// Kiểm tra xem trò chơi đã kết thúc chưa (ví dụ: không còn quân cờ)
			bool botHasPieces = false;
			bool enemyHasPieces = false;

			for (int i = 0; i < 5; i++)
			{
				for (int j = 0; j < 5; j++)
				{
					if (board[i, j] != null)
					{
						if (board[i, j].Color == botColor)
							botHasPieces = true;
						else
							enemyHasPieces = true;
					}
				}
			}

			return !botHasPieces || !enemyHasPieces;
		}

		private List<chess> GetPiecesByColor(chess[,] board, Color color)
		{
			List<chess> pieces = new List<chess>();

			for (int i = 0; i < 5; i++)
			{
				for (int j = 0; j < 5; j++)
				{
					if (board[i, j] != null && board[i, j].Color == color)
					{
						pieces.Add(board[i, j]);
					}
				}
			}

			return pieces;
		}

		private chess[,] CloneBoard(chess[,] board)
		{
			chess[,] clone = new chess[5, 5];

			for (int i = 0; i < 5; i++)
			{
				for (int j = 0; j < 5; j++)
				{
					if (board[i, j] != null)
					{
						clone[i, j] = new chess(board[i, j].X, board[i, j].Y, board[i, j].Color);
					}
				}
			}

			return clone;
		}

		private List<(int, int)> GetValidMoves(chess piece, chess[,] boardPieces)
		{
			List<(int, int)> moves = new List<(int, int)>();

			// Các hướng đi hợp lệ (trái, phải, lên, xuống, chéo)
			int[,] directions = {
				{ 0, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 },
				{ 1, 1 }, { 1, -1 }, { -1, 1 }, { -1, -1 }
			};

			for (int i = 0; i < directions.GetLength(0); i++)
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
