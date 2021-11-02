using System.Collections.Generic;
using UnityEngine;

namespace Om
{
    public class PlayerListViewController : MonoBehaviour
    {
        [SerializeField] private GameObject _contentView;
        [SerializeField] private GameObject _playerContextElemPrefab;
        
        private List<PlayerContextElem> _playerElems = new List<PlayerContextElem>();
        public void AddPlayer(string userName)
        {
            var inst = Instantiate(_playerContextElemPrefab, _contentView.transform);
            var elemContoller = inst.GetComponent<PlayerContextElem>();
            elemContoller.Assign(userName);

            _playerElems.Add(elemContoller);
        }

        public void RemovePlayer(string userName)
        {
            var foundPlayer = _playerElems.Find(x => x.PlayerName == userName);
            _playerElems.Remove(foundPlayer);

            Destroy(foundPlayer.gameObject);
        }
    }
}