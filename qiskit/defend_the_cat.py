# arbitrary state: https://github.com/Qiskit/qiskit-tutorials/blob/master/tutorials/circuits/3_summary_of_quantum_operations.ipynb
from qiskit import QuantumRegister, ClassicalRegister, QuantumCircuit, execute, Aer
import numpy as np


def sample_outputs(qbit_data, circuit_data, sample_size=100):
    qbit_count = qbit_data.shape[0]
    amplitudes = np.empty((qbit_count, 2), dtype=np.complex)
    for qbit_idx in range(qbit_count):
        amplitudes[qbit_idx] = get_amplitudes(*qbit_data[qbit_idx])
        print("amplitudes of qbit {}: {}".format(qbit_idx, amplitudes[qbit_idx]))

    combined_amplitudes = amplitudes[0]
    for qbit_idx in range(1, qbit_count, 1):
        combined_amplitudes = np.kron(combined_amplitudes, amplitudes[qbit_idx])
    print("combined amplitudes: {}".format(combined_amplitudes))

    qreg = QuantumRegister(qbit_count)
    creg = ClassicalRegister(qbit_count)
    qc = QuantumCircuit(qreg, creg)
    qc.initialize(combined_amplitudes, [qreg[i] for i in range(qbit_count)])

    # TODO: apply gates
    qc.measure(qreg, creg)
    print(qc.draw(output='text'))

    job = execute(qc, Aer.get_backend('qasm_simulator'), shots=sample_size)
    counts = job.result().get_counts(qc)
    sample_results = np.zeros(combined_amplitudes.shape, dtype=np.uint8)
    for outcome in counts:
        idx = int(outcome, 2)
        sample_results[idx] = counts[outcome]
        print("{} (index {}) count: {}".format(outcome, idx, counts[outcome]))
    return sample_results

def get_amplitudes(theta, phi): # from bloch sphere
    amp = np.empty(2, dtype=np.complex)
    half_theta = theta * 0.5
    amp[0] = np.cos(half_theta)
    amp[1] = (np.cos(phi) + 1j * np.sin(phi)) * np.sin(half_theta)
    normalize_amplitudes(amp)
    return amp

def normalize_amplitudes(amp): # so that sum of amplitudes-squared equals 1
    sum_amp_squared = get_sum_of_amp_squared(amp)
    amp /= np.sqrt(sum_amp_squared)

def get_sum_of_amp_squared(amp):
    return (amp * np.conjugate(amp)).sum()

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
"""
