//  
// Copyright (c) 2005-2009 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 

using System;
using System.Collections;
using System.Collections.Generic;
using Hd.Portal;
using Tp.RequestServiceProxy;

namespace Hd.Tests
{
	public class RequestDataFactory : IDataFactory
	{
		public IList Retieve(string hql, int? pageIndex, int? pageSize, object[] parameters)
		{
			List<RequestDTO> list = GetList();
			return list;
		}

		public IList Retieve(string hql, object[] parameters)
		{
			throw new NotImplementedException();
		}

		public IList RetieveAll()
		{
			List<RequestDTO> list = GetList();
			return list;
		}

		private static List<RequestDTO> GetList()
		{
			var list = new List<RequestDTO>();

			for (int i = 0; i < 100; i ++)
			{
				RequestDTO requestDto = GetRequestDTO(i);
				list.Add(requestDto);
			}
			return list;
		}

		private static RequestDTO GetRequestDTO(int index)
		{
			var requestDto = new RequestDTO
			                 	{
			                 		ID = index,
			                 		Name = "Request " + index,
			                 		Description = "Hello, world",
			                 		EntityStateID = 1,
			                 		SourceType = RequestSourceEnum.Email,
			                 		RequestTypeID = 2
			                 	};
			return requestDto;
		}

		public object Retrieve(int? identity)
		{
			return GetRequestDTO(identity.Value);
		}

		public object RetrieveWithRecache(int? identity, bool recache)
		{
			return Retrieve(identity);
		}

		public int RetrieveCount(string hql, object[] parameters)
		{
			return 0;
		}
	}
}