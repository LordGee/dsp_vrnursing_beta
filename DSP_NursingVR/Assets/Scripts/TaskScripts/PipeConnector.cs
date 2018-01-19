using UnityEngine;


public class PipeConnector : MonoBehaviour
{
    public int connectorIndex;
    private bool connected { get; set; }

    private void OnTriggerExit(Collider _col) {
        gameObject.GetComponentInParent<Pipe>().Disconnected();
        connected = false;
    }

    private void OnTriggerStay(Collider _col) {
        if (_col.tag == ConstantController.TAG_PIPE_CONNECTOR) {
            if (_col.GetComponentInParent<Pipe>().CheckConnectedSupply()) {
                gameObject.GetComponentInParent<Pipe>().Connected();
                connected = true;
            } else if (gameObject.GetComponentInParent<Pipe>().CheckConnectedSupply()) {
                // Keep object as is
            } else { 
                gameObject.GetComponentInParent<Pipe>().Disconnected();
                connected = false;
            }
        }
    }
}