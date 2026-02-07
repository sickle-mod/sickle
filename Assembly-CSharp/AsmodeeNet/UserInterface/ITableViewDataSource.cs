using System;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x020007CF RID: 1999
	public interface ITableViewDataSource
	{
		// Token: 0x0600392B RID: 14635
		int GetNumberOfCellsInTableView(TableView tableView);

		// Token: 0x0600392C RID: 14636
		float GetHeightForCellIndexInTableView(TableView tableView, int index);

		// Token: 0x0600392D RID: 14637
		TableViewCell GetCellForIndexInTableView(TableView tableView, int index);
	}
}
