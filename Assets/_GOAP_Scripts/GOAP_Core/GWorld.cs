//Sealed class
using System.Collections.Generic;
using GOAP_Resources;

namespace GOAP_Core
{
    public sealed class GWorld
    {
        //Singleton
        private static readonly GWorld instance = new GWorld();
        private static WorldStates world;

        private static Dictionary<string, ResourceList> resourcesQueueDictionary =
            new Dictionary<string, ResourceList>();

        static GWorld()
        {
            world = new WorldStates();
            resourcesQueueDictionary.Add(ListsEnum.ListBuddies.ToString(), 
                new ResourceList(ResourcesEnum._none_.ToString(), StatesEnum._none_.ToString(), StatesEnum._none_.ToString(), world));
            resourcesQueueDictionary.Add(ListsEnum.ListFlowers.ToString(), 
                new ResourceList(ResourcesEnum.ResourceFlowers.ToString(), StatesEnum.state_free_flower.ToString(), StatesEnum._none_.ToString(), world));

        }

        private GWorld()
        {

        }

        public ResourceList GetResourceQueueDictionary(string type)
        {
            return resourcesQueueDictionary[type];
        }

        public ResourceList GetResourceQueueDictionary(ListsEnum type)
        {
            return GetResourceQueueDictionary(type.ToString());
        }

        public static GWorld Instance => instance;

        public WorldStates GetWorld()
        {
            return world;
        }
    }
}