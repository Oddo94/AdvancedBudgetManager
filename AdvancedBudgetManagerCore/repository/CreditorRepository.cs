using AdvancedBudgetManagerCore.model.entity;
using AdvancedBudgetManagerCore.utils.database;
using AdvancedBudgetManagerCore.utils.enums;
using AdvancedBudgetManagerCore.utils.exception;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace AdvancedBudgetManagerCore.repository {
    public class CreditorRepository : ICreditorRepository {
        private IDatabaseConnection dbConnection;
        private string sqlStatementGetCreditorsByUserId =
            @"SELECT creditorID, creditorName FROM creditors c
            INNER JOIN users_creditors uc ON c.creditorID = uc.creditor_ID
            WHERE uc.user_ID = @userId";

        public CreditorRepository(IDatabaseConnection dbConnection) {
            this.dbConnection = dbConnection;
        }

        public IEnumerable<Creditor> GetAll() {
            throw new NotImplementedException();
        }

        public Creditor GetById(long id) {
            throw new NotImplementedException();
        }

        public Creditor Insert(Creditor entity) {
            throw new NotImplementedException();
        }

        public Creditor Update(Creditor entity) {
            throw new NotImplementedException();
        }

        public bool Delete(long id) {
            throw new NotImplementedException();
        }

        public IEnumerable<Creditor> getAllCreditorsByUserId(long userId) {
            using (MySqlConnection conn = (MySqlConnection)dbConnection.GetConnection()) {
                try {
                    MySqlCommand getAllCreditorsByUserIdCommand = new MySqlCommand(sqlStatementGetCreditorsByUserId, conn);
                    getAllCreditorsByUserIdCommand.Parameters.AddWithValue("@userId", userId);

                    conn.Open();
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(getAllCreditorsByUserIdCommand);
                    DataTable retrievedCreditors = new DataTable();

                    dataAdapter.Fill(retrievedCreditors);

                    List<Creditor> creditorsList = new List<Creditor>();
                    foreach (DataRow creditorRow in retrievedCreditors.Rows) {
                        long creditorId = -1;
                        string creditorName = string.Empty;

                        long.TryParse(creditorRow.ItemArray[0].ToString(), out creditorId);
                        creditorName = creditorRow.ItemArray[1].ToString();

                        Creditor creditor = new Creditor(creditorId, creditorName);
                        creditorsList.Add(creditor);
                    }

                    return creditorsList;

                } catch (MySqlException ex) {
                    int errorCode = ex.Number;
                    String message;

                    if (errorCode == 1042) {
                        message = "Unable to connect to the database! Please check the connection and try again.";
                    } else {
                        message = "An error occurred while retrieving data! Please try again.";
                    }

                    throw new AdvancedBudgetManagerException(ExceptionCategory.Persistence, message, ex);
                }
            }
        }
    }
}
