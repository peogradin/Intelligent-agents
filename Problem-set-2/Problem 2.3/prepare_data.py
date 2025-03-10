#%%
import os
import re
import tensorflow as tf
import shutil

dataset_dir = "aclImdb_v1/aclImdb"
train_dir = os.path.join(dataset_dir, "train")
test_dir = os.path.join(dataset_dir, "test")

batch_size = 32
seed = 42

raw_train_ds = tf.keras.utils.text_dataset_from_directory(
    train_dir, batch_size=batch_size, validation_split=0.2, subset="training", seed=seed
)

val_ds = tf.keras.utils.text_dataset_from_directory(
    train_dir, batch_size=batch_size, validation_split=0.2, subset="validation", seed=seed
)

test_ds = tf.keras.utils.text_dataset_from_directory(
    test_dir, batch_size=batch_size
)

def clean_text(text):
    """Remove HTML tags, multiple spaces, and extra tabs from text."""
    text = re.sub(r"<br\s*/?>", " ", text)  # Ersätt <br> med mellanslag
    text = re.sub(r"\s+", " ", text)  # Ersätt multipla mellanslag med ett
    text = text.strip()  # Ta bort eventuella mellanslag i början/slutet
    return text

def save_dataset_to_file(dataset, filepath):
    with open(filepath, "w", encoding="utf-8") as f:
        for text_batch, label_batch in dataset:
            for text, label in zip(text_batch.numpy(), label_batch.numpy()):
                text_decoded = text.decode("utf-8")
                text_cleaned = clean_text(text_decoded)
                f.write(f"{text_cleaned}\t{label}\n")

save_dataset_to_file(raw_train_ds, "train_set.txt")
save_dataset_to_file(val_ds, "val_set.txt")
save_dataset_to_file(test_ds, "test_set.txt")

print("Datasets saved successfully!")

# %%
