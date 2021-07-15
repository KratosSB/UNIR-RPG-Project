using UnityEngine;

namespace GOAP_Resources
{
    public class FixedEnumNames : PropertyAttribute { }

    public enum ResourcesEnum
    {
        _none_,
        ResourceBuddies,
        ResourceFlowers
    }

    public enum ListsEnum
    {
        _none_,
        ListBuddies,
        ListFlowers
    }
    
    #region NotInUse
    
    public enum StatesEnum_
    {

    }

    public enum BeliefsNameEnum
    {

    }

    public enum GoalsNameEnum
    {

    }

    #endregion

    public enum StatesEnum
    {
        _none_,
        idle,
        goal_killPlayer,
        state_attackingPlayer,
        belief_attackingPlayer,
        order_attackPlayer,
        goal_eat_flower,
        state_free_flower,
        belief_starving,
        belief_full_feeded,
        goal_wander
    }

    public enum ActionTypeEnum
    {
        Movement,
        Fight
    }
}