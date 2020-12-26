# arbitrary state: https://quantumcomputing.stackexchange.com/questions/1413/how-to-create-an-arbitrary-state-in-qiskit-for-a-local-qasm-simulator
from qiskit import QuantumRegister, ClassicalRegister, QuantumCircuit, execute, Aer
import numpy as np


def sample_outputs(qbit_data, circuit_data):
    qbit_count = qbit_data.shape[0]
    amplitudes = np.empty((qbit_count, 2), dtype=np.complex)
    for qbit_idx in range(qbit_count):
        amplitudes[qbit_idx] = get_amplitudes(*qbit_data[qbit_idx])
        print("amplitudes of {}: {}".format(qbit_idx, amplitudes[qbit_idx]))
        test_amplitudes(amplitudes[qbit_idx])

def get_amplitudes(theta, phi): # from bloch sphere
    amp = np.empty(2, dtype=np.complex)
    half_theta = theta * 0.5
    amp[0] = np.cos(half_theta)
    amp[1] = (np.cos(phi) + 1j * np.sin(phi)) * np.sin(half_theta)
    return amp

def test_amplitudes(amp):
    print(amp[0] * np.conjugate(amp[0]) + amp[1] * np.conjugate(amp[1]))

"""
def color2(qreg, circuit):
    # (a)
    circuit.cx(qreg[0], qreg[5])
    circuit.cx(qreg[2], qreg[5])
    # (b)
    circuit.cx(qreg[0], qreg[6])
    circuit.cx(qreg[3], qreg[6])
    # (c)
    circuit.cx(qreg[1], qreg[7])
    circuit.cx(qreg[2], qreg[7])
    # (d)
    circuit.ccx(qreg[5], qreg[6], qreg[8])
    circuit.ccx(qreg[7], qreg[8], qreg[4])

for input in all_inputs:
    qreg = QuantumRegister(9)
    creg = ClassicalRegister(1)
    circuit = QuantumCircuit(qreg, creg)

    # initialize the inputs
    for i in range(4):
        if input[i] == '1':
            circuit.x(qreg[3 - i])

    color2(qreg, circuit)
    circuit.measure(qreg[4], creg)
    # print(circuit.draw(output='text'))

    job = execute(circuit, Aer.get_backend('qasm_simulator'), shots=1)
    counts = job.result().get_counts(circuit)
    for outcome in counts:
        print("(input vertices) |{}> --> |{}> (output)".format(input, outcome))
"""