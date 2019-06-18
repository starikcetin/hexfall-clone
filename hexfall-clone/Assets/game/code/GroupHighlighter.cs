using UnityEngine;

namespace starikcetin.hexfallClone.game
{
    public class GroupHighlighter : MonoBehaviour
    {
        public void Highlight(Group group)
        {
            gameObject.SetActive(true);
            transform.position = group.Center;
            RotateForOrientation(group.Orientation);
        }

        private void RotateForOrientation(GroupOrientation orientation)
        {
            var zRot = orientation == GroupOrientation.TwoRight ? 0 : 180;

            transform.rotation = Quaternion.AngleAxis(zRot, new Vector3(0,0,1));
        }
    }
}
