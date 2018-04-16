using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupImageMap : MonoBehaviour {

    [System.Serializable]
	public class PowerupImageEntry
    {
        public PowerupType type;
        public Sprite image;
    }
    public PowerupImageEntry[] powerupImages;

    public Sprite GetImage(PowerupType type)
    {
        foreach(PowerupImageEntry p in powerupImages)
        {
            if (type == p.type)
            {
                return p.image;
            }
        }
        return null;
    }

}
