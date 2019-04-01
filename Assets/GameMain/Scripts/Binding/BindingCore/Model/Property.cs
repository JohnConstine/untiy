using System;

namespace SG1
{
	public abstract class Property : EventArgs
	{
		private event EventHandler<Property> m_OnValueChange;
		
		public event EventHandler<Property> OnValueChange
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
				m_OnValueChange.Invoke(this, this);
			}
		}
	}
}