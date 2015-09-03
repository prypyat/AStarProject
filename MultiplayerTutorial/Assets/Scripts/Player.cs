using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public float speed = 5f;

    private float lastSynchronizationTime = 0f;
    private float syncDelay = 0f;
    private float syncTime = 0f;
    private Vector3 syncStartPosition = Vector3.zero;
    private Vector3 syncEndPosition = Vector3.zero;

    private Node _current;
    private Node _target;

    private List<Node> _path; 

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
        /*Vector3 syncPosition = Vector3.zero;
        Vector3 syncVelocity = Vector3.zero;
        if (stream.isWriting)
        {
            syncPosition = transform.position;
            stream.Serialize(ref syncPosition);

            syncPosition = rigidbody.velocity;
            stream.Serialize(ref syncVelocity);
        }
        else
        {
            stream.Serialize(ref syncPosition);
            stream.Serialize(ref syncVelocity);

            syncTime = 0f;
            syncDelay = Time.time - lastSynchronizationTime;
            lastSynchronizationTime = Time.time;

            syncEndPosition = syncPosition + syncVelocity * syncDelay;
            syncStartPosition = rigidbody.position;
        }*/
    }

    void Awake()
    {
        lastSynchronizationTime = Time.time;
    }

    void Update()
    {
        /*if (networkView.isMine)
        {
            InputMovement();
            UpdateMovement();
            InputColorChange();

        }
        else
        {
            SyncedMovement();
        }*/

        InputMovement();
        UpdateMovement();
        InputColorChange();
    }

    private void InputMovement() {

        if (Input.GetMouseButtonDown(0)) {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit)) {

                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Map")) {

                    Node targetPoint = Level.Instance.Map.GetNode(Level.Instance.World2Grid(hit.point));

                    if (targetPoint.Walkable) {
                        
                        Node startPoint = _target;

                        bool fromStart = false;

                        if (startPoint == null) {
                            startPoint = _current;
                            fromStart = true;
                        }

                        _path = Level.Instance.Astar.FindPath(startPoint, targetPoint);

                        if (fromStart) {
                            _path.RemoveAt(0);
                        }

                        _target = _path[0];
                        _path.RemoveAt(0);
                    }
                    
                }

            }
        }

    }

    private void UpdateMovement() {
        
        _current = Level.Instance.Map.GetNode(Level.Instance.World2Grid(transform.position));

        if (_target == null) {
            return;
        }

        Vector3 target = Level.Instance.Grid2World(_target.Position);

        if (Vector3.Distance(target, transform.position) < 0.1f) {

            _current = _target;

            if (_path.Count > 0) {
                _target = _path[0];
                _path.RemoveAt(0);
            } else {
                _target = null;
            }

        }

        if (_target != null) {

            target = Level.Instance.Grid2World(_target.Position);

            transform.position += (target - transform.position).normalized*(speed*0.01f);
        }

    }

    /*
    private void InputMovement()
    {
        if (Input.GetKey(KeyCode.W))
            rigidbody.MovePosition(rigidbody.position + Vector3.forward * speed * Time.deltaTime);

        if (Input.GetKey(KeyCode.S))
            rigidbody.MovePosition(rigidbody.position - Vector3.forward * speed * Time.deltaTime);

        if (Input.GetKey(KeyCode.D))
            rigidbody.MovePosition(rigidbody.position + Vector3.right * speed * Time.deltaTime);

        if (Input.GetKey(KeyCode.A))
            rigidbody.MovePosition(rigidbody.position - Vector3.right * speed * Time.deltaTime);
    }
    */
    private void SyncedMovement()
    {
        syncTime += Time.deltaTime;

        //rigidbody.position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
    }


    private void InputColorChange()
    {
        if (Input.GetKeyDown(KeyCode.R))
            ChangeColorTo(new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
    }
	
	/*private void InputRotation()
	{
		rigidbody.MoveRotation();
	}
	
	private void syncedRotation()
	{
		rigidbody.MoveRotation();
	}*/
	
    [RPC] void ChangeColorTo(Vector3 color)
    {
        renderer.material.color = new Color(color.x, color.y, color.z, 1f);

        if (networkView.isMine)
            networkView.RPC("ChangeColorTo", RPCMode.OthersBuffered, color);
    }
}
