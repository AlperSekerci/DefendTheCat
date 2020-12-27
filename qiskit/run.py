from unity_handler import UnityHandler
from defend_the_cat import sample_outputs

PORT = 17920
unity_handler = UnityHandler(port=PORT)
while True:
    try:
        unity_handler.receive()
        results = sample_outputs(unity_handler.qbit, unity_handler.circuit)
        unity_handler.send(results)
    except:
        print("got exception")
        unity_handler.reset()
