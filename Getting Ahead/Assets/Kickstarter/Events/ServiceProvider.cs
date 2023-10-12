using UnityEngine;

namespace Kickstarter.Events
{
    [RequireComponent(typeof(IServiceProvider))]
    public class ServiceProvider : MonoBehaviour
    {
        [SerializeField] private Service service;

        public Service Service
        {
            get
            {
                return service;
            }
            set
            {
                service = value;
            }
        }
    }
}
