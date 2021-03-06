import socket
import numpy as np


class UnityHandler:
    def __init__(self,
                 port=15920,
                 team_count=4,
                 qbit_count=2,
                 ):
        self.team_count = team_count
        self.qbit_count = qbit_count
        self.each_qbit_float_ct = 2 # theta and phi of bloch sphere
        self.float_size = 4
        self.port = port

        self.state_bytes = self.float_size * self.qbit_count * self.each_qbit_float_ct
        self.circuit_bytes = self.team_count * self.qbit_count
        self.total_input_bytes = self.state_bytes + self.circuit_bytes

        self.sock = None
        self.__start_listening()

        # region Data
        self.qbit = np.empty((self.qbit_count, self.each_qbit_float_ct), dtype=np.float32)
        self.circuit = np.empty((self.team_count, self.qbit_count), dtype=np.uint8)
        # endregion

    def __separate_data(self, data):
        start = 0
        end = self.state_bytes
        self.qbit[:] = np.frombuffer(data[start:end], dtype=np.float32).reshape(self.qbit.shape)
        print("qbit: {}".format(self.qbit))
        start = end
        end += self.circuit_bytes
        self.circuit[:] = np.frombuffer(data[start:end], dtype=np.uint8).reshape(self.circuit.shape)
        print("circuit: {}".format(self.circuit))

    def receive(self):
        data = np.frombuffer(self.connection.recv(self.total_input_bytes), dtype=np.uint8)
        self.__separate_data(data)

    def send(self, data):
        self.connection.send(data)

    def reset(self):
        if self.sock is not None:
            try:
                print("shutting down the socket")
                self.sock.shutdown(socket.SHUT_RDWR)
                self.sock.close()
            except:
                print("some error during socket shutdown")
        self.__start_listening()

    def __start_listening(self):
        print("will start listening")
        self.sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.sock.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
        server_address = ('', self.port)
        print('starting up on \'{}\' port {}'.format(*server_address))
        self.sock.bind(server_address)
        self.sock.listen(1)
        print('waiting for a connection')
        self.connection, client_address = self.sock.accept()
        print('connection from', client_address)
