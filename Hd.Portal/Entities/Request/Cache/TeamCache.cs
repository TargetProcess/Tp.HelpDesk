// 
// Copyright (c) 2005-2008 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Tp.RequestServiceProxy;

namespace Hd.Portal.Entities.Request
{
	public class TeamCache : ChildEntityCache
	{
		protected override void ProcessEntries(Hashtable hashtable, int[] ids)
		{
			RequestService requestServiceWse = ServiceManager.GetService<RequestService>();
			TeamListItemDTO[] teamListItemDTOS = requestServiceWse.RetrieveTeamsForRequests(ids);

			foreach (int id in ids)
			{
				List<Team> teams = new List<Team>();

				foreach (TeamListItemDTO dto in teamListItemDTOS)
				{
					if (dto.AssignableID == id)
					{
						Team team = new Team();

						Role role = DataPortal.Instance.Retrieve(typeof (Role), dto.RoleID) as Role;

						if (role != null)
						{
							team.ActorName = role.Name;
						}

						User user = DataPortal.Instance.Retrieve(typeof (User), dto.UserID) as User;

						if (user != null)
						{
							team.UserName = string.Format("{0} {1}", user.FirstName, user.LastName);
						}

						teams.Add(team);
					}
				}

				AppendGroupToHashtable(id, teams, hashtable);
			}
		}
	}
}