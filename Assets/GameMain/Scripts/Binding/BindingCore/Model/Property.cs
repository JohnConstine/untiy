using System;
using UnityEngine;

namespace SG1
{
	public abstract class Property
	{
		private event Action m_OnValueChange;
		
		public event Action OnValueChange
		{
			add
			{
				m_OnValueChange += value;
			}
			remove
			{
				m_OnValueChange -= value;
			}
		}

		protected void OnValueChanged()
		{
			if (m_OnValueChange != null)
			{
				m_OnValueChange.Invoke();
			}
		}

		public abstract object GetRowValue();
	}
}